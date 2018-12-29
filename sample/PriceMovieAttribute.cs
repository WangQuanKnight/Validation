using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ValidationSample
{
    public class PriceMovieAttribute : ValidationAttribute, IClientModelValidator
    {
        private int _maxPrice;
        private int _minPrice;

        public PriceMovieAttribute(int maxPrice,int minPrice)
        {
            _maxPrice = maxPrice;
            _minPrice = minPrice; 
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Movie movie = validationContext.ObjectInstance as Movie;

            if(movie.Genre == Genre.Classic && (movie.Price >= _maxPrice || movie.Price <= _minPrice)){
                return new ValidationResult(GetErrorMessage()); 
            }

            return ValidationResult.Success;
        }


        public void AddValidation(ClientModelValidationContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-PriceMovie", GetErrorMessage());

            var maxPrice = _maxPrice.ToString(CultureInfo.InvariantCulture);
            var minPrice = _minPrice.ToString(CultureInfo.InvariantCulture);
            MergeAttribute(context.Attributes, "data-val-pricemovie-maxprice", maxPrice);
            MergeAttribute(context.Attributes, "data-val-pricemovie-minprice", minPrice); 
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }

        private string GetErrorMessage()
        {
            return $"Classic movies must have a Price between {_maxPrice} and {_minPrice}";
        }
    }
}
