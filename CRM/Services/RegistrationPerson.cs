using DTO;
using Models;
using Enums;
using AbstractClasses;

namespace Services
{
    public class AdminRegistration : Registration
    {
        public AdminRegistration(List<Person> person) : base(PersonRegistration) { }

        public override Result<bool> RegistrationPerson(InputUserDto data)
        {
            string textEror = string.Empty; bool fieldValidation = true;
            Result<bool> result;

            if (string.IsNullOrEmpty(data.FirstName))
            {
                textEror += "Text box FirstName is empty, please enter First name\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.LastName))
            {
                textEror += "Text box LastName is empty, please enter you Last name\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Patronymic))
            {
                textEror += "Text box Patronymic is empty, please enter you Patronymic\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Patronymic))
            {
                textEror += "Text box DateOfBirth is empty, please enter you Date of birth\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Login))
            {
                textEror += "Text box Login is empty, please enter you Login\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Password))
            {
                textEror += "Text box Password is empty, please enter you Password\n";
                fieldValidation = false;
            }


            if (!fieldValidation)
                result = new Result<bool>
                {
                    Error = ErrorStatus.ArgumentNull,
                    IsSuccessfully = fieldValidation,
                    TextError = textEror,
                    Payload = false
                };
            else
            {
                PersonRegistration.Add(
                    new Person(data, Roles.Admin)
                    {
                        Id = Guid.NewGuid(),
                        Status = StatusUser.Accepted
                    });

                result = new Result<bool>
                {
                    Error = ErrorStatus.Success,
                    IsSuccessfully = fieldValidation,
                    TextError = "Registration completed successfully\n",
                    Payload = true
                };

            }


            return result;

        }
    }


    public class UserRegistration : Registration
    {
        public UserRegistration(List<Person> persons) : base(persons) { }

        public override Result<bool> RegistrationPerson(InputUserDto data)
        {
            string textEror = string.Empty; bool fieldValidation = true;
            Result<bool> result;

            if (string.IsNullOrEmpty(data.FirstName))
            {
                textEror += "Text box FirstName is empty, please enter First name\n";
                fieldValidation = false;
            }
            if (string.IsNullOrEmpty(data.LastName))
            {
                textEror += "Text box LastName is empty, please enter you Last name\n";
                fieldValidation = false;
            }
            if (string.IsNullOrEmpty(data.Patronymic))
            {
                textEror += "Text box Patronymic is empty, please enter you Patronymic\n";
                fieldValidation = false;
            }
            if (string.IsNullOrEmpty(data.Patronymic))
            {
                textEror += "Text box DateOfBirth is empty, please enter you Date of birth\n";
                fieldValidation = false;
            }
            if (string.IsNullOrEmpty(data.Login))
            {
                textEror += "Text box Login is empty, please enter you Login\n";
                fieldValidation = false;
            }
            if (string.IsNullOrEmpty(data.Password))
            {
                textEror += "Text box Password is empty, please enter you Password";
                fieldValidation = false;
            }
            if (!fieldValidation)
                result = new Result<bool> {
                    Error = ErrorStatus.ArgumentNull,
                    IsSuccessfully = fieldValidation,
                    TextError = textEror,
                    Payload = false };
            else
            {
                PersonRegistration.Add(
                    new Person(data, Roles.User) { 
                        Id = Guid.NewGuid(), 
                        Status = StatusUser.Pending });

                result = new Result<bool> {
                    Error = ErrorStatus.Success,
                    IsSuccessfully = fieldValidation,
                    TextError = "Registration completed successfully\n",
                    Payload = true
                };

            }
            
            
            return result;

        }
    }


    public class ManagerRegistration : Registration
    {
        public ManagerRegistration(List<Person> persons) : base(persons) { }
        public override Result<bool> RegistrationPerson(InputUserDto data)
        {
            string textEror = string.Empty; bool fieldValidation = true;
            Result<bool> result;

            if (string.IsNullOrEmpty(data.FirstName))
            {
                textEror += "Text box FirstName is empty, please enter First name\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.LastName))
            {
                textEror += "Text box LastName is empty, please enter you Last name\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Patronymic))
            {
                textEror += "Text box Patronymic is empty, please enter you Patronymic\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Patronymic))
            {
                textEror += "Text box DateOfBirth is empty, please enter you Date of birth\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Login))
            {
                textEror += "Text box Login is empty, please enter you Login\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Password))
            {
                textEror += "Text box Password is empty, please enter you Password\n";
                fieldValidation = false;
            }


            if (!fieldValidation)
                result = new Result<bool>
                {
                    Error = ErrorStatus.ArgumentNull,
                    IsSuccessfully = fieldValidation,
                    TextError = textEror,
                    Payload = false
                };
            else
            {
                PersonRegistration.Add(
                    new Person(data, Roles.Manager)
                    {
                        Id = Guid.NewGuid(),
                        Status = StatusUser.Accepted
                    });

                result = new Result<bool>
                {
                    Error = ErrorStatus.Success,
                    IsSuccessfully = fieldValidation,
                    TextError = "Registration completed successfully\n",
                    Payload = true
                };

            }


            return result;

        }
    }


    public class ModeratorRegistration : Registration
    {
        public ModeratorRegistration(List<Person> persons) : base(persons) { }
        public override Result<bool> RegistrationPerson(InputUserDto data)
        {
            string textEror = string.Empty; bool fieldValidation = true;
            Result<bool> result;

            if (string.IsNullOrEmpty(data.FirstName))
            {
                textEror += "Text box FirstName is empty, please enter First name\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.LastName))
            {
                textEror += "Text box LastName is empty, please enter you Last name\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Patronymic))
            {
                textEror += "Text box Patronymic is empty, please enter you Patronymic\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Patronymic))
            {
                textEror += "Text box DateOfBirth is empty, please enter you Date of birth\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Login))
            {
                textEror += "Text box Login is empty, please enter you Login\n";
                fieldValidation = false;
            }
            else if (string.IsNullOrEmpty(data.Password))
            {
                textEror += "Text box Password is empty, please enter you Password\n";
                fieldValidation = false;
            }


            if (!fieldValidation)
                result = new Result<bool>
                {
                    Error = ErrorStatus.ArgumentNull,
                    IsSuccessfully = fieldValidation,
                    TextError = textEror,
                    Payload = false
                };
            else
            {
                PersonRegistration.Add(
                    new Person(data, Roles.Moderator)
                    {
                        Id = Guid.NewGuid(),
                        Status = StatusUser.Accepted
                    });

                result = new Result<bool>
                {
                    Error = ErrorStatus.Success,
                    IsSuccessfully = fieldValidation,
                    TextError = "Registration completed successfully\n",
                    Payload = true
                };

            }


            return result;

        }
    }
}
