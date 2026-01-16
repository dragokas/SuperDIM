using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DazUnpacker.source
{
    internal enum FilterTypes
    {
        FilterDateType
    }

    interface IFilter
    {
        FilterTypes FilterType { get; }
    }

    internal class DateFilter : IFilter
    {
        public FilterTypes FilterType { get; }
        internal DateFilterAction Action { get; set; }
        internal DateFilterDirection Direction { get; set; }
        internal DateTime Date { get; set; }

        internal DateFilter(DateFilterAction action, DateFilterDirection direction, DateTime date)
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

    internal enum DateFilterAction
    {
        Created
    }

    internal enum DateFilterDirection
    {
        Before,
        After,
        Equal
    }

    internal static class FilterManager
    {
        private static List<IFilter> filterList = new List<IFilter>();

        internal static void AddDateFilter(DateFilterAction action, DateFilterDirection direction, DateTime date)
        {
            filterList.Add(new DateFilter(action, direction, date));
        }

        internal static void ClearFilters()
        {
            filterList.Clear();
        }

        internal static bool HasFilters()
        {
            return filterList.Count > 0;
        }

        internal static bool IsPassed(DateTime dateCreated)
        {
            foreach (IFilter filter in filterList)
            {
                if (filter.FilterType == FilterTypes.FilterDateType)
                {
                    if (!((DateFilter)filter).IsPassed(DateFilterAction.Created, dateCreated))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }


}
