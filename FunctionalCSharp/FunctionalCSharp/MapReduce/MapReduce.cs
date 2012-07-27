using System.Collections.Generic;

namespace MapReduce
{
    class MapReduce
    {
        public delegate IEnumerable<KeyValuePair<TOK, TOV>> Map<TIK, TIV, TOK, TOV>(TIK key, TIV value);

        public delegate IEnumerable<KeyValuePair<TOK, TOV>> Reduce<TIK, TIV, TOK, TOV>(TIK key, IEnumerable<TIV> value);

        public static IEnumerable<KeyValuePair<TOK, TOV>> Run<TIK, TIV, TTK, TTV, TOK, TOV>(
        Map<TIK, TIV, TTK, TTV> map,
        Reduce<TTK, TTV, TOK, TOV> reduce,
        IEnumerable<KeyValuePair<TIK, TIV>> input)
        {
            //
            // Map and group all at once
            //
            var tempStorage = new Dictionary<TTK, List<TTV>>();
            foreach (var inputPair in input)
                foreach (var tempPair in map(inputPair.Key, inputPair.Value))
                {
                    List<TTV> vals;
                    if (!tempStorage.TryGetValue(tempPair.Key, out vals))
                    {
                        vals = new List<TTV>();
                        tempStorage.Add(tempPair.Key, vals);
                    }
                    vals.Add(tempPair.Value);
                }

            //
            // Reduce
            //
            foreach (var tempPair in tempStorage)
                foreach (var outPair in reduce(tempPair.Key, tempPair.Value))
                    yield return outPair;
        }
    }
}
