using Models;
using Enums;

namespace Services
{
    class ErorMassageServices
    {
        private readonly List<Person> PersonsList;
        public ErorMassageServices(List<Person> personsList)
        {
            PersonsList = personsList;
        }

        public List<Massage> massageError;


        public Result<string> IsNullOrEmptyMassage<T>(T text)
        {
            string str = text.ToString();

            if (string.IsNullOrEmpty(str))
            {
                return new Result<string>
                {
                    Error = ErrorStatus.ArgumentNull,
                    IsSuccessfully = false,
                    TextError = "Please fill in this text box",
                    ResultOperation = "Cloase"
                };
            }

            return new Result<string>()
            {
                Error = ErrorStatus.Success,
                IsSuccessfully = true,
                ResultOperation = "Open"
            };
        }


        public Result<string> NotFound(Guid Id)
        {
            var Search = PersonsList.FirstOrDefault(x => x.Id == Id);

            if (Search == null)
            {
                return new Result<string>
                {
                    Error = ErrorStatus.NotFound,
                    IsSuccessfully = false,
                    TextError = "Not found",
                    ResultOperation = string.Empty
                };
            }
            return new Result<string>
            {
                Error = ErrorStatus.Success,
                IsSuccessfully = true,
                TextError = string.Empty,
                ResultOperation = string.Empty
            };
        }


        public Result<Massage> SearchMassage(Massage massage)
        {

            if (string.IsNullOrEmpty(massage.Name))
            {
                return new Result<Massage>
                {
                    Error = ErrorStatus.ArgumentNull,
                    IsSuccessfully = false,
                    TextError = "Please fill in the text box \"Name\"",
                    ResultOperation = null
                };
            }
            else if (string.IsNullOrEmpty(massage.Theme))
            {
                return new Result<Massage>
                {
                    Error = ErrorStatus.ArgumentNull,
                    IsSuccessfully = false,
                    TextError = "Please fill in the text box \"Theme\"",
                    ResultOperation = null
                };
            }
            else if (string.IsNullOrEmpty(massage.Text))
            {
                return new Result<Massage>
                {
                    Error = ErrorStatus.ArgumentNull,
                    IsSuccessfully = false,
                    TextError = "Please fill in the text box \"Text\"",
                    ResultOperation = null
                };
            }

            return new Result<Massage>
            {
                Error = ErrorStatus.Success,
                IsSuccessfully = true,
                TextError = string.Empty,
                ResultOperation = massage
            };
        }
        public Result<string> WrongText(string[] correctText, string inputText)
        {
            for (int i = 0; i < correctText.Length; i++)
                if (inputText == correctText[i])
                {
                    return new Result<string>()
                    {
                        Error = ErrorStatus.Success,
                        IsSuccessfully = true,
                        ResultOperation = string.Empty
                    };
                }


            return new Result<string>
            {
                Error = ErrorStatus.ArgumentNull,
                IsSuccessfully = false,
                TextError = "Your text is wrong. Please try again",
                ResultOperation = string.Empty
            };
        }
    }
}
