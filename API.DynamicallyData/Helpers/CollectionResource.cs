using API.DynamicallyData.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace API.DynamicallyData.Helpers
{
    public class CollectionResource
    {
        public int CurrentPage { get; }
        public int TotalCount { get; }
        public int PageSize { get; } = 1;
        public IEnumerable<ExpandoObject> Items { get; }
        public IDictionary<string, string> Keys { get; }
        public int TotalPages => (int)Math.Ceiling(this.TotalCount / (double)this.PageSize);
        public bool HasPrevious => (this.CurrentPage > 1);
        public bool HasNext => (this.CurrentPage < this.TotalPages);

        private CollectionResource(IEnumerable<ExpandoObject> Items, IDictionary<string, string> keys, int currentPage, int pageSize, int totalCount)
        {
            this.Items = Items;
            this.Keys = keys;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize; ;
            this.TotalCount = totalCount;
        }

        public static CollectionResource Create(IEnumerable<IEnumerable<Element>> rows, int currentPage, int pageSize, int totalCount)
        {
            if (rows == null)
            {
                return new CollectionResource(new List<ExpandoObject>(), new Dictionary<string, string>(), currentPage, pageSize, totalCount);
            }

            var expandoObjectList = CreateObject(rows);
            var keys = rows.First().ToDictionary(i => i.key, i => CreateSpaceBetweenCapitalLetters(i.key));

            return new CollectionResource(expandoObjectList, keys, currentPage, pageSize, totalCount);
        }

        private static List<ExpandoObject> CreateObject(IEnumerable<IEnumerable<Element>> rows)
        {
            var expandoObjectList = new List<ExpandoObject>();

            foreach (var row in rows)
            {

                var dataObject = new ExpandoObject();
                foreach (var property in row)
                {
                    ((IDictionary<string, object>)dataObject).Add(property.key, property.Value);
                }

                expandoObjectList.Add(dataObject);
            }

            return expandoObjectList;
        }

        private static string CreateSpaceBetweenCapitalLetters(string key)
        {
            var values = key.Select((c, i) => (char.IsUpper(c) &&
                                                i > 0 &&
                                                (char.IsLower(key[i - 1]) || (i < key.Count() - 1 && char.IsLower(key[i + 1])))) ? " " + c : c.ToString());

            return string.Join(string.Empty, values);
        }
    }
}
