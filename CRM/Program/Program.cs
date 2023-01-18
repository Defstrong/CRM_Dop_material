using DTO;
using Models;
using Services;
using Enums;
using AbstractClasses;

var _personsList = new List<Person>();
var _personsLoanRequestsList = new List<Loan>();
var _employeePrepaidExpenseRequests = new List<PrepaidExpense>();

var _massages = new List<Massage>();
var _write = new Write();
var _login = new Login(_personsList);
var _registration = new UserRegistration(_personsList);
var _createAdmin = new AdminRegistration(_personsList);
var _createModerator = new ModeratorRegistration(_personsList);
bool _sendMassageFromManagerForOverdueRequest = false;
var _editUser = new DtoEditUser();


var _userServices = new UserServices(_personsList);
var _adminServices = new AdminServices(_personsList, _personsLoanRequestsList, _massages);
var _moderatorServices = new ModeratorServices(_personsList);
var _managerServices = new ManagerServices(_personsLoanRequestsList);
var _accountantServices = new AccountantServices(_personsList, _employeePrepaidExpenseRequests);
var _allStatistic = new Statistic();

var _managerMassage = new ManagerMassage(_massages, _personsList, _personsLoanRequestsList);
var _userMassage = new UserMassage(_massages, _personsList, _personsLoanRequestsList);
var _adminMassage = new AdminMassage(_massages, _personsList, _personsLoanRequestsList);
int countAcceptedRequestRegistration = 0, countRefuseRequestRegistration = 0;

var _adminLoanServices = new AdminLoanServices(_personsList, _personsLoanRequestsList);
var _userLoanServices = new UserLoanServices(_personsList, _personsLoanRequestsList);
string helpStr = string.Empty;


_personsList.Add(new Person
{
    FirstName = "Bob",
    LastName = "Bob",
    Age = 20,
    Patronymic = "Bob",
    Login = "Bob",
    Password = "1",
    Role = Roles.User,
    Status = StatusUser.Accepted,
    Id = Guid.NewGuid()
});
_personsList.Add(new Person
{
    FirstName = "Li",
    LastName = "Li",
    Age = 20,
    Patronymic = "Li",
    Login = "Li",
    Password = "1",
    Role = Roles.User,
    Status = StatusUser.Accepted,
    Id = Guid.NewGuid()
});
_personsList.Add(new Person{ 
    FirstName = "Admin", 
    LastName = "Admin", 
    Age = 20, 
    Patronymic = "Admin", 
    Login = "Admin", 
    Password = "Admin", 
    Role = Roles.Admin,
    Id = Guid.NewGuid(),
    Status = StatusUser.Accepted});
_personsList.Add(new Person
{
    FirstName = "Moderator",
    LastName = "Moderator",
    Age = 20,
    Patronymic = "Moderator",
    Login = "Moderator",
    Password = "Moderator",
    Role = Roles.Moderator,
    Id = Guid.NewGuid(),
    Status = StatusUser.Accepted
});
_personsList.Add(new Person
{
    FirstName = "Manager",
    LastName = "Manager",
    Age = 20,
    Patronymic = "Manager",
    Login = "Manager",
    Password = "Manager",
    Role = Roles.Manager,
    Id = Guid.NewGuid(),
    Status = StatusUser.Accepted
});

var dtoSendMassage = new DtoSendMassage();
var dto = new InputUserDto();
var massage = new Massage();
var dutys = new Loan();
int i = 1;


while(i++ < 100)
{
    PaymentDateArrivedOrNot();
    WriteCountStatusRequestRegistration(_allStatistic.countAcceptedRequestRegistration, "accepted");
    WriteCountStatusRequestRegistration(_allStatistic.countRefuseRequestRegistration, "refuse");
    dto = new InputUserDto();
    string inputCommand = string.Empty;
    Command(ref inputCommand);
    if (inputCommand == "Registration")
        Registration();
    else if (inputCommand == "Login")
    {
        Login(ref dto); Password(ref dto); 
        var result = _login.LoginPerson(dto);
        Console.WriteLine(result.TextError);
        if(result.IsSuccessfully)
        {
            Guid idPerson = result.Payload.Id;
            Command(ref inputCommand);
            if (dto.Role == Roles.User)
                UserAction(inputCommand, idPerson);
            else if (dto.Role == Roles.Admin)
                AdminAction(inputCommand, idPerson);
            else if (dto.Role == Roles.Moderator)
                ModeratorAction(inputCommand);
            else if (dto.Role == Roles.Manager)
                ManagerAction(inputCommand, idPerson);
            else if(dto.Role == Roles.Employee)
            {
                int idx = _personsList.FindIndex(x => x.Id.Equals(dto.Id));
                if (inputCommand == "Personal area")
                    PersonalAreaEmployee(_personsList[idx]);
                if(inputCommand == "Prepaid expense")
                {
                    PrepaidExpense(idx);
                }
                if (_personsList[idx].Responsibility == ResponsibilityPerson.Accountant)
                {
                    AccountantAction(inputCommand, _personsList[idx]);
                }
            }
        }
    }
    else if (inputCommand == "Create admin")
        CreateAdmin();
}

void PrepaidExpense(int idx)
{
    var askForAnAdvance = new PrepaidExpense()
    {
        Id = Guid.NewGuid(),
        IdEmloyee = _personsList[idx].Id,
        FirstName = _personsList[idx].FirstName,
        LastName = _personsList[idx].LastName,
        Patronymic = _personsList[idx].Patronymic,
    };

    Console.Write("Please enter reason for the advance: ");
    askForAnAdvance.ReasonForTheAdvance = Console.ReadLine();
    Console.Write("Please enter count months: ");
    askForAnAdvance.CountMonths = int.Parse(Console.ReadLine());
    _employeePrepaidExpenseRequests.Add(askForAnAdvance);
}

void AccountantAction(string commandAccountant, Person dataAccountant)
{
    if(commandAccountant == "Employee salary")
        EmployeeSalary();
    else if(commandAccountant == "Request for advance")
    {
        ShowAllIdRequestForAdvance();
        Guid idRequestForAdvance = EnterId();
        string choiceAccountant = Console.ReadLine();
        _accountantServices.RequestForAdvance(idRequestForAdvance, choiceAccountant);
    }

}

void PaymentDateArrivedOrNot()
{
    foreach(var ii in _personsList)
    {
        if(ii.Role == Roles.Employee)
        {
            DateTime date = ii.DataPassportEmployeeAndSalary.SalaryPaymentDate;

            if (DateTime.Now >= date)
            {
                if(ii.DataPassportEmployeeAndSalary.SalaryReceived == false)
                    ii.DataPassportEmployeeAndSalary.SalaryReceived = true;
                else
                    ii.DataPassportEmployeeAndSalary.SalaryReceived = false;

                ii.DataPassportEmployeeAndSalary.SalaryPaymentDate = date.AddMonths(1);
            }
        }
    }
}

void ShowAllIdRequestForAdvance()
{
    foreach(var ii in _employeePrepaidExpenseRequests)
    {
        if (ii.Status == StatusRequestForAdvance.Pending)
            Console.WriteLine($"Id: {ii.Id}\nFirs name: {ii.FirstName}" +
                $" Last name: {ii.LastName} Pantronymic: {ii.Patronymic}" +
                $"\nReason for the advance:{ii.ReasonForTheAdvance}");
    }
}

void EmployeeSalary()
{
    ShowAllIdEmployee();

    Guid idEmployee = EnterId();
    int salaryEmployee = int.Parse(Console.ReadLine());
    _accountantServices.EmployeeSalary(idEmployee, salaryEmployee);
}

void ShowAllIdEmployee()
{
    foreach(var ii in _personsList)
        if(ii.Role == Roles.Employee)
            Console.WriteLine(ii.Id);
}

void PersonalAreaEmployee(Person personalAreaEmployee)
{
    Console.WriteLine(personalAreaEmployee.DataPassportEmployeeAndSalary.ToString());
}

void UserAction(string commandUser, Guid userId)
{
    int idx = FindIndex(userId);
    if(!_sendMassageFromManagerForOverdueRequest)
        _managerMassage.OverdueRequest(ref _sendMassageFromManagerForOverdueRequest);
    dto.Id = userId;

    if (commandUser == "Get loan")
    {
        AmounMoney(); Payday();
        _userLoanServices.Loan(dto, idx);
    }
    else if (commandUser == "Status dutys")
        StatusDuty(userId);
    else if (commandUser == "Personal area")
    {
        PersonalArea(idx);
    }
    else if (commandUser == "My massages")
        MyMassages(_managerMassage, userId);
    else if (commandUser == "Delete profile")
        _userServices.DeleteProfile(userId);
    else if (commandUser == "Edit profile")
        EditProfile(userId);
    else if (commandUser == "Send massage")
        SendMassage(_userMassage, idx);
    else if (commandUser == "Debt off")
        PayTheDebtOff(userId);
}

void PersonalArea(int idx)
{
    if (_personsList[idx].Status == StatusUser.Accepted)
        Console.WriteLine(_personsList[idx].ToString());
    else
        Console.WriteLine("User is not found");
}

void AdminAction(string commandAdmin, Guid adminId)
{
    int idx = FindIndex(adminId);
    if (commandAdmin == "Show user")
    {
        ShowAllUsersId(_personsList, StatusUser.Accepted);
        Guid IdUser = EnterId();
        int idxUser = FindIndex(IdUser);
        ShowUser(idxUser);
    }
    else if (commandAdmin == "Show users")
        ShowUsers(StatusUser.Accepted);
    else if (commandAdmin == "Send massage")
        SendMassage(_adminMassage, idx);
    else if (commandAdmin == "My massages")
        MyMassages(_adminMassage, adminId);
    else if (commandAdmin == "Edit person")
        EditPerson();
    else if (commandAdmin == "Delete person")
        DeletePerson();
    else if (commandAdmin == "Block person")
        BlockPerson();
    else if (commandAdmin == "Create loan for user")
        CreateLoanForUser();
    else if (commandAdmin == "Pay off user debts")
        PayOffUserDebts();
    else if (commandAdmin == "Сreate an employee")
        UserToEmployee();
    else if (commandAdmin == "Duplicate user")
        DuplicatePerson();
    else if (commandAdmin == "Duplicate loan")
        DuplicateLoan();

}

void DuplicateLoan()
{
    ShowAllIdUserRequestLoan();

    Guid idLoan = EnterId();
}

void DuplicatePerson()
{
    ShowAllPersonId();
    Guid idPerson = EnterId();
    Console.Write("Enter new login for person: ");
    string newLoginForDuplicatePerson = Console.ReadLine();
    Console.Write("Enter new password for person: ");
    string newPasswordForDuplicatePerson = Console.ReadLine();
    _adminServices.DuplicatePerson(idPerson, newLoginForDuplicatePerson, newPasswordForDuplicatePerson);
}


DtoDataPassportAndSallary EnterDataPassportAndSalary(ref DtoDataPassportAndSallary DataPassport)
{
    Console.Write("Enter First name: ");
    DataPassport.FirstName = Console.ReadLine();
    Console.Write("Enter Last name: ");
    DataPassport.LastName = Console.ReadLine();
    Console.Write("Enter Patronymic: ");
    DataPassport.Patronymic = Console.ReadLine();
    Console.Write("Enter Date of birth: ");
    DataPassport.DateOfBirth = DateTime.Parse(Console.ReadLine());
    Console.Write("Enter Place of residence: ");
    DataPassport.PlaceOfResidence = Console.ReadLine();
    Console.Write("Enter Marital status: ");
    DataPassport.MaritalStatus = Console.ReadLine();
    Console.Write("Enter Phone number: ");
    DataPassport.PhoneNumber = Console.ReadLine();
    Console.Write("Enter Salary: ");
    DataPassport.Salary = int.Parse(Console.ReadLine());
    return DataPassport;
}


void ShowAllIdUsersLoan()
{
    foreach (var ii in _personsLoanRequestsList)
    {
            Console.WriteLine(ii.Id);
            _write.Status(ii.StatusDuty);
            _write.WriteDutys(ii.ToString());
    }
}
void UserToEmployee()
{
    var DataPassportEmployee = new DtoDataPassportAndSallary();
    ShowAllUserId();
    Guid idUser = EnterId();
    Console.Write("Who do you want to turn the user into: ");
    string ResponsibilityEmployee = Console.ReadLine();
    _adminServices.UserToEmployee(EnterDataPassportAndSalary(ref DataPassportEmployee), idUser, ResponsibilityEmployee);
}

void PayOffUserDebts()
{
    ShowAllIdUserRequestLoanForAdmin();
    Guid idTransactionUser = EnterId();

    _adminLoanServices.PayTheDebtOff(idTransactionUser);
}

void ShowAllIdUserRequestLoanForAdmin()
{
    foreach (var ii in _personsLoanRequestsList)
    {
        if (ii.StatusDuty == StatusLoan.Accepted)
        {
            Console.WriteLine("\n" + ii.Id);
            _write.Status(ii.StatusDuty);
            _write.WriteDutys(ii.ToString());
        }
    }
}

void CreateLoanForUser()
{
    ShowAllUserId();
    Guid idUser = EnterId();
    int idxUser = FindIndex(idUser);
    dto.Id = _personsList[idxUser].Id;
    AmounMoney(); Payday();
    _adminLoanServices.Loan(dto, idxUser);
}

void EditPerson()
{
    ShowAllPersonId();
    int i = 0;
    Guid idPerson = EnterId();
    DtoEditUser dtoEditUser = new DtoEditUser() { Id = idPerson };
    while (i++ <= 6)
    {
        string atribut = string.Empty, change = string.Empty;
        atribut = EnterAnother(atribut, "change");
        change = EnterAnother(change, "changer");
        if (atribut == "First name")
            dtoEditUser.FirstName = change;
        else if (atribut == "Last name")
            dtoEditUser.LastName = change;
        else if (atribut == "Patronymic")
            dtoEditUser.Patronymic = change;
        else if (atribut == "Age")
            dtoEditUser.Age = int.Parse(change);
        else if (atribut == "Login")
            dtoEditUser.Login = change;
        else if (atribut == "Password")
            dtoEditUser.Password = change;
        else if (atribut == string.Empty)
            break;
    }
    _adminServices.EditPerson(dtoEditUser);
}

void BlockPerson()
{
    ShowAllPersonId();
    Guid idPerson = EnterId();
    _adminServices.BlockPerson(idPerson);
}

void DeletePerson()
{
    ShowAllPersonId();
    Guid idPerson = EnterId();
    _adminServices.DeletePerson(idPerson);
}

void ShowAllPersonId()
{
    for (int i = 0; i < _personsList.Count; i++)
        if (_personsList[i].Status == StatusUser.Accepted)
            Console.WriteLine($"\t\t\tRole: {_personsList[i].Role}\nName: {_personsList[i].FirstName} - Id: {_personsList[i].Id}");
}


void ManagerAction(string commandManager, Guid managerId)
{
    int idx = FindIndex(managerId);
    if (commandManager == "Statistic")
        StatisticLoan();
    else if (commandManager == "Requests")
        LoanRequestsManager();
    else if (commandManager == "Send massage")
        SendMassage(_managerMassage, idx);
    else if (commandManager == "My massages")
        MyMassages(_managerMassage, managerId);
}

void LoanRequestsManager()
{
    if (_personsLoanRequestsList.Count != 0)
    {
        ShowUsersIdTransaction();
        ShowUsersLoan();
        string Choice = string.Empty; Guid Id = Guid.Empty;
        _write.Choice(ref Id, ref Choice);
        int idxUser = _personsLoanRequestsList.FindIndex(x => x.Id.Equals(Id));

        _managerServices.ChoiceLoanRequest(idxUser, Choice);
        if (Choice == "Accepted")
            _allStatistic.countAcceptedRequestLoan++;
        else if (Choice == "Refuse")
            _allStatistic.countRefuseRequestLoan++;
    }
    else
        Console.WriteLine("\nYou don't have Loan requests");
}
void SendMassage(MassageService personMassage, int idx)
{
    string DefinitionPerson = _personsList[idx].Role.ToString();

    var result = personMassage.SendMassage(InputDataSendMassage(dtoSendMassage, idx));

    Console.WriteLine(result.TextError);
}

DtoSendMassage InputDataSendMassage(DtoSendMassage dataSendMassage, int indexUser)
{
    string AdminOrManager = string.Empty;
    string DefinitionPerson = _personsList[indexUser].Role.ToString();
    string inputText = string.Empty;
    string WriteText;
    if(DefinitionPerson == "User")
    {
        string[] ManagerAdmin = { "Manager", "Admin" };
        Console.Write("Who do you want to send a message to Admin or Manager: ");
        dataSendMassage.AdminOrManager = Console.ReadLine();
        Console.Write("Please enter theme massage: ");
        dataSendMassage.Theme = Console.ReadLine();
        Console.Write("Please enter text massage: ");
        dataSendMassage.Text = Console.ReadLine();
        dataSendMassage.Id = _personsList[indexUser].Id;
        dataSendMassage.FirstName = _personsList[indexUser].FirstName;
    }
    else
    {
        ShowAllUserId();
        Guid EnterUserId;
        string EnterUserIdStr;
        EnterUserId = EnterId();
        Console.Write("Please enter theme massage: ");
        dataSendMassage.Theme = Console.ReadLine();
        Console.Write("Please enter text massage: ");
        dataSendMassage.Text = Console.ReadLine();
        dataSendMassage.Id = EnterUserId;
    }
    return dataSendMassage;
}

void Registration()
{
    string atributRegistration = string.Empty;
    FirstName(ref dto);
    LastName(ref dto);
    Patronymic(ref dto); 
    DateOfBirth(ref dto);
    Login(ref dto);
    Password(ref dto);
    var result = _registration.RegistrationPerson(dto);
    Console.WriteLine(result.TextError);
}

void PayTheDebtOff(Guid userId)
{
    ShowAllIdUserRequestLoan(userId);
    _userLoanServices.PayTheDebtOff(EnterIdRequestForTheDebtOff());
}
Guid EnterIdRequestForTheDebtOff()
{
    Guid IdRequest = Guid.Empty;
    Console.Write("Please enter Id request: ");
    string str = Console.ReadLine();
    if (!string.IsNullOrEmpty(str) && str.Length == 36)
        IdRequest = Guid.Parse(str);

    return IdRequest;
}
void ShowAllIdUserRequestLoan(Guid userId)
{
    foreach (var ii in _personsLoanRequestsList)
    {
        if (ii.IdSender == userId && ii.StatusDuty == StatusLoan.Accepted)
        {
            Console.WriteLine(ii.Id);
            _write.Status(ii.StatusDuty);
            _write.WriteDutys(ii.ToString());
        }
    }
}
void StatusDuty(Guid userId)
{
    bool thePresenceOfDebt = false;

    foreach (var ii in _personsLoanRequestsList)
    {
        if (ii.IdSender == userId)
        {
            _write.Status(ii.StatusDuty);
            _write.WriteDutys(ii.ToString());
            thePresenceOfDebt = true;
        }
    }
    if (!thePresenceOfDebt)
        Console.WriteLine("\nYou don't have loan");
}

void MyMassages(MassageService showMassage, Guid idPerson)
{
    ShowAllIdMassages(idPerson);
    bool presenceOfMessage = false;
    int idx = _personsList.FindIndex(x => x.Id.Equals(idPerson));
    Console.WriteLine(_personsList[idx].Role);
        foreach (var ii in _massages)
        {
            if (_personsList[idx].Role == Roles.User)
            {
                if(ii.IdRecipient == idPerson)
                {
                    presenceOfMessage = true;
                    Console.WriteLine(ii.ToString());
                }
            }
            else
            {
                Console.WriteLine(ii.ToString());
                presenceOfMessage = true;
            }
        }
    if (presenceOfMessage)
        ShowMassage(showMassage);
    else
        Console.WriteLine("\nYou dot't have massages\n");
}
void ShowMassage(MassageService showMassage)
{
    Guid IdMassage = EnterId();
    var result = showMassage.MyMassages(IdMassage);
    Console.WriteLine(result.TextError);
    if (result.IsSuccessfully)
        Console.WriteLine($"{result.Payload.ToString()}\n {result.Payload.Text}");
}

void ShowAllIdMassages(Guid id)
{
    int idx = _personsList.FindIndex(x => x.Id.Equals(id));
    if(idx != -1)
        foreach (var ii in _massages)
        {
            if (_personsList[idx].Role == Roles.User)
            {
                if (ii.IdRecipient == id)
                    Console.WriteLine(ii.Id);
            }
            else
            {
                if (ii.Role == _personsList[idx].Role)
                    Console.WriteLine(ii.Id);
            }
        }
}
void EditProfile(Guid id)
{
    int i = 0;
    DtoEditUser dtoEditProfile = new DtoEditUser() { Id = id};
    while (i++ <= 6)
    {
        string atribut = string.Empty, change = string.Empty;
        atribut = EnterAnother(atribut, "change");
        change = EnterAnother(change, "changer");
        if (atribut == "First name")
            dtoEditProfile.FirstName = change;
        else if (atribut == "Last name")
            dtoEditProfile.LastName = change;
        else if (atribut == "Patronymic")
            dtoEditProfile.Patronymic = change;
        else if (atribut == "Age")
            dtoEditProfile.Age = int.Parse(change);
        else if (atribut == "Login")
            dtoEditProfile.Login = change;
        else if (atribut == "Password")
            dtoEditProfile.Password = change;
        else if (atribut == string.Empty)
            break;
    }
    _userServices.EditProfile(dtoEditProfile);
}
//

//Manager Methods
void StatisticLoan()
{
    Console.WriteLine("Accepted Request: " + _allStatistic.countAcceptedRequestLoan);
    Console.WriteLine("Refuse Request: " + _allStatistic.countRefuseRequestLoan);
}
void ShowUsersLoan()
{
    foreach (var ii in _personsLoanRequestsList)
    {
        if (ii.StatusDuty == StatusLoan.Pending)
            Console.WriteLine(ii.ToString());
    }
}
//

void ModeratorAction(string commandAdmin)
{

    if(commandAdmin == "Requests")
    {
        ShowAllUsersId(_personsList, StatusUser.Pending);
        ShowUsers(StatusUser.Pending);
        string Choice = string.Empty; Guid Id = Guid.Empty;
        _write.Choice(ref Id, ref Choice);
        int idx = _personsList.FindIndex(x => x.Id.Equals(Id));

        var result = _moderatorServices.ChoiceRequest(Id, Choice);
        if (Choice == "Refuse")
        {
            _allStatistic.countRefuseRequestRegistration++;
            string textСauseRefusalOfRegistration = string.Empty;
            _personsList[idx].CauseRefuseRegistration = СauseRefusalOfRegistration(textСauseRefusalOfRegistration);
        }
        else if (Choice == "Accepted")
            _allStatistic.countAcceptedRequestRegistration++;
        Console.WriteLine(result.TextError);
    }
}
//Moderator Methods
void ShowAllUsersId(List<Person> personsId, StatusUser status)
{
    bool presence = false;
    for (int i = 0; i < personsId.Count; i++)
    {
        if (personsId[i].Role == Roles.User && personsId[i].Status == status)
        {
            Console.WriteLine($"{personsId[i].FirstName} {personsId[i].LastName}: Id: {personsId[i].Id.ToString()}");
            presence = true;
        }
    }
    if (!presence)
        throw new Exception("You don't have requests");
}
//

//Admin Methods
Guid EnterId()
{
    Guid IdRequest = Guid.Empty;
    Console.Write("Please enter id: ");
    string str = Console.ReadLine();
    if (!string.IsNullOrEmpty(str) && str.Length == 36)
        IdRequest = Guid.Parse(str);

    return IdRequest;
}
void ShowUser(int idx)
{
    Console.WriteLine(_personsList[idx].ToString());
}
void ShowUsers(StatusUser status)
{
    foreach(var ii in _personsList)
    {
        if (ii.Role == Roles.User && ii.Status == status)
            Console.WriteLine(ii.ToString());
    }
}
/////////////////

void ShowAllUserId()
{
    for (int i = 0; i < _personsList.Count; i++)
        if (_personsList[i].Status == StatusUser.Accepted && _personsList[i].Role == Roles.User)
            Console.WriteLine($"Name: {_personsList[i].FirstName} - Id: {_personsList[i].Id}");
}
void WriteCountStatusRequestRegistration(int count, string status)
{
    Console.WriteLine($"\nCount {status} registration: {count}\n");
}
string СauseRefusalOfRegistration(string Сause)
{
    Console.Write("Enter your cause: ");
    Сause = Console.ReadLine();

    return Сause;
}
string СauseRefusalOfDuty(string cause)
{
    Console.Write("Enter your cause: ");
    cause = Console.ReadLine();
    return cause;
}
void ShowMyTransaction(Guid id)
{
    foreach (var ii in _personsLoanRequestsList)
    {
        if (ii.IdSender == id)
            Console.WriteLine($"Name: {ii.Name} - Id Transaction: {ii.Id}");
    }
}
void ShowAllUserIdLoan()
{
    bool presence = false;
    for (int i = 0; i < _personsLoanRequestsList.Count; i++)
    {
        if (_personsLoanRequestsList[i].StatusDuty == StatusLoan.Pending)
        {
            Console.WriteLine($"Name: {_personsLoanRequestsList[i].Name} - Id: {_personsLoanRequestsList[i].Id}");
            presence = true;
        }
    }
    if (!presence)
        throw new Exception("You don't have requests");
}
void CreateAdmin()
{
    FirstName(ref dto); LastName(ref dto);
    Patronymic(ref dto); DateOfBirth(ref dto);
    Login(ref dto); Password(ref dto);
    var result = _createAdmin.RegistrationPerson(dto);
    Console.WriteLine(result.TextError);
}
void CreateModerator()
{
    FirstName(ref dto); LastName(ref dto);
    Patronymic(ref dto); DateOfBirth(ref dto);
    Login(ref dto); Password(ref dto);
    _createModerator.RegistrationPerson(dto);
}
void FirstName(ref InputUserDto inputUserDtoFirstName)
{
    Console.Write("First name: ");
    inputUserDtoFirstName.FirstName = Console.ReadLine();

}
void LastName(ref InputUserDto inputUserDtoLastName)
{
    Console.Write("Last name: ");
    inputUserDtoLastName.LastName = Console.ReadLine();
}
void Patronymic(ref InputUserDto inputUserDtoPatronymic)
{
    Console.Write("Patronymic: ");
    inputUserDtoPatronymic.Patronymic = Console.ReadLine();
}
void DateOfBirth(ref InputUserDto inputUserDtoDateOfBirth)
{
    unchecked
    {
        Console.Write("Date of birth: ");
        helpStr = Console.ReadLine();
        if(!string.IsNullOrEmpty(helpStr))
            inputUserDtoDateOfBirth.DateOfBirth = DateTime.Parse(helpStr);

    }
}
void Login(ref InputUserDto inputUserDtoLogin)
{
    Console.Write("Login: ");
    inputUserDtoLogin.Login = Console.ReadLine();
}
void Password(ref InputUserDto inputUserDtoPassword)
{
    Console.Write("Password: ");
    inputUserDtoPassword.Password = Console.ReadLine();
}
void AmounMoney()
{
    Console.Write("Amoun money: ");
    dto.AmountMoney = int.Parse(Console.ReadLine());
}
void Payday()
{
    Console.Write("Payday: ");
    dto.Payday = DateTime.Parse(Console.ReadLine());
}
string Command(ref string command)
{
    Console.Write("Enter your command:> ");
    command = Console.ReadLine();
    return command;
}
int FindIndex(Guid id)
{
    int idx = _personsList.FindIndex(x => x.Id.Equals(id));
    return idx;
}
string EnterAnother(string another, string change)
{
    Console.Write($"Enter {change}: ");
    another = Console.ReadLine();
    Console.WriteLine();
    return another;
}
void ShowUsersIdTransaction()
{
    foreach (var ii in _personsLoanRequestsList)
    {
        if(ii.StatusDuty == StatusLoan.Pending)
            Console.WriteLine($"Name: {ii.Name} - Id Transaction: {ii.Id}");
    }
}