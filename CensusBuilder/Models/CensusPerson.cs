namespace CensusBuilder.Models
{
    public class CensusPerson
    {
        public int Id { get; internal set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public SexEnum Sex { get; set; }
        public int Age { get; set; }
        public string Birthplace { get; set; }
        public string Occupation { get; internal set; }
        public int LineNumber { get; internal set; }

        public string GetBirthRange(Census census)
        {
            int? minYear;
            int? maxYear;

            if (census.AgeIsRounded && Age > census.ChildAgeEnd)
            {
                //Get max age of person
                //e.g. "15" can be 15-19
                //"30" could be 30-34
                int minAge = Age;
                int maxAge = Age + census.AgeRoundingMaxError;

                maxYear = census.Date.Year - minAge;
                minYear = census.Date.Year - maxAge - 1;
            }
            else
            {
                //lets say 6 June 1860
                //subject is 20
                //could be born 6 June 1840 and today is their birthday 19 -> 20
                //or born 5 June 1840 and today they are 20 + 1 day
                //or born 7 June 1839 and tomorrow they will be 21
                //so roughly speaking ... 
                maxYear = census.Date.Year - Age;
                minYear = maxYear - 1;
            }

            return $"{minYear.ToString() ?? "Unknown"}-{maxYear.ToString() ?? "Unknown"}";
        }
    }
}