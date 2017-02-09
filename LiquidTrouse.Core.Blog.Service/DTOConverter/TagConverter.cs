using LiquidTrouse.Core.Blog.DataAccess.Domain;
using LiquidTrouse.Core.Blog.Service.DTO;
using System.Collections;
using System.Collections.Generic;

namespace LiquidTrouse.Core.Blog.Service.DTOConverter
{
    public class TagConverter
    {
        public TagConverter() { }

        public IList ToDomainObject(TagInfo[] tagInfos)
        {
            var tagList = new List<Tag>();
            if (tagInfos != null && tagInfos.Length > 0)
            {
                foreach (var tagInfo in tagInfos)
                {
                    var tag = new Tag();
                    tag.TagId = tagInfo.TagId;
                    tag.DisplayName = tagInfo.DisplayName.ToLower().Trim();
                    tag.UsedCount = tagInfo.UsedCount;
                    tag.LastUsedDatetime = tagInfo.LastUsedDatetime;
                    tagList.Add(tag);
                }
            }
            return tagList;
        }

        public Tag ToDomainObject(TagInfo tagInfo)
        {
            var tag = ToDomainObject(new TagInfo[] { tagInfo })[0] as Tag;
            return tag;
        }

        public TagInfo[] ToDataTransferObject(IList tags)
        {
            var tagInfoList = new List<TagInfo>();
            if (tags != null && tags.Count > 0)
            {
                foreach (Tag tag in tags)
                {
                    var tagInfo = new TagInfo();
                    tagInfo.TagId = tag.TagId;
                    tagInfo.DisplayName = tag.DisplayName.ToLower().Trim();
                    tagInfo.UsedCount = tag.UsedCount;
                    tagInfo.LastUsedDatetime = tag.LastUsedDatetime;
                    tagInfoList.Add(tagInfo);
                }
            }
            return tagInfoList.ToArray();
        }

        public TagInfo ToDataTransferObject(Tag tag)
        {
            var tagInfo = ToDataTransferObject(new List<Tag>() { tag })[0];
            return tagInfo;
        }
    }
}
