using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DazUnpacker.source
{
    public enum FilterTypes
    {
        FilterDateType,
        FilterContentType
    }

    public interface IFilter
    {
        FilterTypes FilterType { get; set; }
    }

    public enum DateFilterAction
    {
        Created
    }

    public enum DateFilterDirection
    {
        Before,
        After,
        Equal
    }

    public class DateFilter : IFilter
    {
        public FilterTypes FilterType { get; set; }
        public DateFilterAction Action { get; set; }
        public DateFilterDirection Direction { get; set; }
        public DateTime Date { get; set; }

        public DateFilter() { }
        public DateFilter(DateFilterAction action, DateFilterDirection direction, DateTime date)
        {
            Action = action;
            Direction = direction;
            Date = date;
            FilterType = FilterTypes.FilterDateType;
        }

        internal bool IsPassed(DateFilterAction action, DateTime fileDate)
        {
            if (action == DateFilterAction.Created)
            {
                if (Direction == DateFilterDirection.After)
                {
                    if (fileDate.Date > Date.Date)
                    {
                        return true;
                    }
                }
                else if (Direction == DateFilterDirection.Before)
                {
                    if (fileDate.Date < Date.Date)
                    {
                        return true;
                    }
                }
                else if (Direction == DateFilterDirection.Equal)
                {
                    if (fileDate.Date == Date.Date)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class ContentFilter : IFilter
    {
        public FilterTypes FilterType { get; set; }
        public ContentTypes ContentType { get; set; }

        public ContentFilter() { }
        public ContentFilter(ContentTypes contentType)
        {
            FilterType = FilterTypes.FilterContentType;
            ContentType = contentType;
        }

        internal bool IsPassed(ContentTypes contentType)
        {
            return contentType == this.ContentType;
        }
    }

    public class FilterManager
    {
        public List<IFilter> FilterList = new List<IFilter>();
        public bool IsDateFilterEnabled = false;
        public bool IsContentTypeFilterEnabled = false;

        public FilterManager() { }

        internal void AddDateFilter(DateFilterAction action, DateFilterDirection direction, DateTime date)
        {
            FilterList.Add(new DateFilter(action, direction, date));
        }

        internal void AddContentTypeFilter(ContentTypes contentType)
        {
            FilterList.Add(new ContentFilter(contentType));
        }

        internal void ClearFilters()
        {
            FilterList.Clear();
        }

        internal bool HasFilters()
        {
            return FilterList.Count > 0;
        }

        internal bool IsPassed(DateTime dateCreated)
        {
            if (!IsDateFilterEnabled)
                return true;

            foreach (IFilter filter in FilterList)
            {
                if (filter is DateFilter dateFilter)
                {
                    if (!dateFilter.IsPassed(DateFilterAction.Created, dateCreated))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal bool IsPassed(ContentTypes contentType)
        {
            if (!IsContentTypeFilterEnabled)
                return true;

            foreach (IFilter filter in FilterList)
            {
                if (filter is ContentFilter contentFilter)
                {
                    if (contentFilter.IsPassed(contentType))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


    }


}
