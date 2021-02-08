using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private Dictionary<string, List<Shirt>> dictShirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
            dictShirts = new Dictionary<string, List<Shirt>>();
            shirts.ForEach(x => {
                string key = $"{x.Color.Id} - {x.Size.Id}";
                if (dictShirts.ContainsKey(key))
                {
                    dictShirts[key].Add(x);
                }
                else
                {
                    dictShirts.Add(key, new List<Shirt>() { x });
                }
            });          

        }


        public SearchResults Search(SearchOptions options)
        {
            // TODO: search logic goes here.
            if (options.Colors == null || options.Colors.Count == 0)
            {
                options.Colors = Color.All;
            }
            if (options.Sizes == null || options.Sizes.Count == 0)
            {
                options.Sizes = Size.All;
            }
            List<Shirt> searchedShirts = new List<Shirt>();
            var searchKeys = (from optsize in options.Sizes
                             from optcolor in options.Colors
                             select $"{optcolor.Id} - {optsize.Id}").ToList();


            searchKeys.ForEach(x =>
            { 
               if (dictShirts.ContainsKey(x))
               {
                    searchedShirts.AddRange(dictShirts[x]);
               }
            
            });

            var colorCounts = getColorCounts(searchedShirts).ToList();
            var sizeCounts = getSizeCounts(searchedShirts).ToList();

            return new SearchResults
            {
                Shirts = searchedShirts,
                ColorCounts = colorCounts,
                SizeCounts = sizeCounts
            };
        }

        private static IEnumerable<SizeCount> getSizeCounts(List<Shirt> searchedShirts)
        {           
            var sizeCounts = searchedShirts
                .GroupBy(sh => sh.Size)
                .Select(size => new SizeCount {
                 Size = size.Key,
                 Count = size.Count()                
                });
            return sizeCounts;          
        }


        private IEnumerable<ColorCount> getColorCounts(List<Shirt> searchedShirts)
        {
            var colorCounts = searchedShirts
                .GroupBy(sh => sh.Color)
                .Select(color => new ColorCount
                {
                    Color = color.Key,
                    Count = color.Count()
                });
            return colorCounts;
          
        }
    }
}