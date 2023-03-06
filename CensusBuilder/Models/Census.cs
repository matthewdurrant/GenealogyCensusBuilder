namespace CensusBuilder.Models
{
    public record Census
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public List<CensusRecord> Records { get; set; } = new();

        public static Census EnglandAndWales1841 => new Census
        {
            Date = DateTime.Parse("6 June 1841"),
            Name = "England and Wales Census, 1841",
            AgeIsRounded = true,
            ChildAgeEnd = 14,
            AgeRoundingMaxError = 4

        };

        public static Census EnglandAndWales1851 => new Census
        {
            Date = DateTime.Parse("30 March 1851"),
            Name = "England and Wales Census, 1851"
        };

        /// <summary>
        /// If the census taker rounded ages. For example, the 1841 census rounds ages down (23  -> 20, 44 -> 40, 58 -> 55 etc.)
        /// </summary>
        public bool AgeIsRounded { get; internal set; }

        /// <summary>
        /// If the census taker rounded ages, the age where accurate ages ends. For example, in the 1841 census, children aged 14 and under had accurate ages.
        /// </summary>
        public int ChildAgeEnd { get; internal set; }

        /// <summary>
        /// If the census taker rounded ages, the maximum error for the age rounding. For example, in the 1841 census, it was 4 (a 24 year old might be recorded as a 20 year old).
        /// </summary>
        public int AgeRoundingMaxError { get; internal set; }

        public IEnumerable<(string Place, IEnumerable<CensusRecord> CensusRecords)> ByPlace()
        {
            return Records.GroupBy(r => r.FullAddress).Select(r => (r.Key, r.AsEnumerable()));
        }
    }
}
