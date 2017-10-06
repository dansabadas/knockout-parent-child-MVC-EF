using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class CheckScoreAttribute : ValidationAttribute
    {
        private readonly decimal _minimumScore;


        public CheckScoreAttribute(double minimumScore) : base("Invalid score.")
        {
            _minimumScore = (decimal) minimumScore;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            decimal score = Score(value != null ? value.ToString() : " ");
            return score < _minimumScore 
              ? new ValidationResult(string.Format("The name score must be greater than or equal to {0}, but was only {1}.", _minimumScore, score)) 
              : ValidationResult.Success;
        }

        private decimal Score(string name)
        {
            if (string.IsNullOrEmpty(name))
                return 1m;

            name = name.Trim();

            int endingLength = -1;
            int startingLength = 0;
            while (startingLength > endingLength)
            {
                startingLength = name.Length;
                name = name.Replace("  ", " ");
                endingLength = name.Length;
            }

            string[] nameParts = name.Split(' ');

            int numberOfParts = nameParts.Length;
            int numberOfCharacters = name.Length - numberOfParts + 1;

            return numberOfCharacters / (decimal)numberOfParts;
        }
    }
}