using DTO;
using Models;
using Services;
using Enums;
using AbstractClasses;

var _personsList = new List<Person>();
var _personsRequestsList = new List<Loan>();
var _massages = new List<Massage>();
var  write = new Write();
var _login = new Login(_personsList);
var _registration = new UserRegistration(_personsList);
var _createAdmin = new AdminRegistration(_personsList);
var _createModerator = new ModeratorRegistration(_personsList);
bool _sendMassageFromManagerForOverdueRequest = false;

var _editUser = new DtoEditUser();

var _userServices = new UserServices(_personsList);
var _adminServices = new AdminServices(_personsList,_personsRequestsList, _massages);
var _moderatorServices = new ModeratorServices(_personsList);
var _managerServices = new ManagerServices(_personsRequestsList);
var _allStatistic = new Statistic();

var _managerMassage = new ManagerMassage(_massages, _personsList, _personsRequestsList);
var _userMassage = new UserMassage(_massages, _personsList, _personsRequestsList);
var _adminMassage = new AdminMassage(_massages, _personsList, _personsRequestsList);
int countAcceptedRequestRegistration = 0, countRefuseRequestRegistration = 0;

var _adminLoanServices = new AdminLoanServices(_personsList, _personsRequestsList);
var _userLoanServices = new UserLoanServices(_personsList, _personsRequestsList);

var _errorMassages = new ErorMassageServices(_personsList);


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

var massage = new Massage();
var dutys = new Loan();
int i = 1;
var dto = new InputUserDto();


while(i++ < 100)
{
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
        Guid idPerson = _login.LoginPerson(dto);
        Command(ref inputCommand);
        if (dto.Role == Roles.User)
        {
            UserAction(inputCommand, idPerson);
        }
        else if (dto.Role == Roles.Admin)
            AdminAction(inputCommand, idPerson);
        else if (dto.Role == Roles.Moderator)
            ModeratorAction(inputCommand);
        else if (dto.Role == Roles.Manager)
            ManagerAction(inputCommand, idPerson);
    }
    else if (inputCommand == "Create admin")
        CreateAdmin();
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
        PersonalArea(idx);
    else if (commandUser == "My massages")
        MyMassages(userId);
    else if (commandUser == "Delete profile")
        _userServices.DeleteProfile(userId);
    else if (commandUser == "Edit profile")
        EditProfile(userId);
    else if (commandUser == "Send massage")
        SendMassage(_userMassage, idx);
    else if (commandUser == "Debt off")
        PayTheDebtOff(userId);
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
    {
        MyMassagesAdmin(adminId);
    }
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
}

void PayOffUserDebts()
{
    ShowAllIdUserRequestLoanForAdmin();
    Guid idTransactionUser = EnterId();

    _adminLoanServices.PayTheDebtOff(idTransactionUser);
}

void ShowAllIdUserRequestLoanForAdmin()
{
    foreach (var ii in _personsRequestsList)
    {
        if (ii.StatusDuty == StatusLoan.Accepted)
        {
            Console.WriteLine("\n" + ii.Id);
            write.Status(ii.StatusDuty);
            write.WriteDutys(ii.ToString());
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
        MyMassagesManager(managerId);
}

void LoanRequestsManager()
{
    if (_personsRequestsList.Count != 0)
    {
        ShowUsersIdTransaction();
        ShowUsersLoan();
        string Choice = string.Empty; Guid Id = Guid.Empty;
        write.Choice(ref Id, ref Choice);
        int idxUser = _personsRequestsList.FindIndex(x => x.Id.Equals(Id));

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

    personMassage.SendMassage(InputDataSendMassage(dto, idx));
}

InputUserDto InputDataSendMassage(InputUserDto dataSendMassage, int indexUser)
{
    string AdminOrManager = string.Empty;
    string DefinitionPerson = _personsList[indexUser].Role.ToString();
    string inputText = string.Empty;
    string WriteText;
    if(DefinitionPerson == "User")
    {
        string[] ManagerAdmin = { "Manager", "Admin" };
        WriteText = "Who do you want to send a message to Admin or Manager: ";
        whileMethodWrongText(WriteText, ref AdminOrManager, ManagerAdmin);
        if (AdminOrManager == "Admin")
            dataSendMassage.Role = Roles.Admin;
        else if (AdminOrManager == "Manager")
            dataSendMassage.Role = Roles.Manager;
        WriteText = "Please enter theme massage: ";
        whileMethodNullOrEmpty(WriteText, ref inputText);
        dataSendMassage.Theme = inputText;
        WriteText = "Please enter text massage: ";
        whileMethodNullOrEmpty(WriteText, ref inputText);
        dataSendMassage.Text = inputText;

        dataSendMassage.Id = _personsList[indexUser].Id;
        dataSendMassage.FirstName = _personsList[indexUser].FirstName;
    }
    else
    {
        ShowAllUserId();
        string EnterUserId;
        Console.Write("Enter user Id: ");
        EnterUserId = Console.ReadLine();
        Console.Write("Please enter theme massage: ");
        dataSendMassage.Theme = Console.ReadLine();
        Console.Write("Please enter text massage: ");
        dataSendMassage.Text = Console.ReadLine();
        dataSendMassage.Id = Guid.Parse(EnterUserId);
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
    _registration.RegistrationPerson(dto);
}


void whileMethodWrongText(string writeText, ref string input, string[] arrData)
{
    input = string.Empty;
    bool breakWhile = false;
    while (breakWhile == false)
    {
        Console.Write(writeText);
        input = Console.ReadLine();
        var eror = _errorMassages.WrongText(arrData, input);
        Console.WriteLine(eror.TextError);
        breakWhile = eror.IsSuccessfully;
    }
}
void whileMethodNullOrEmpty(string writeText, ref string input)
{
    input = string.Empty;
    int i = 0;
    bool breakWhile = false;
    while(breakWhile == false)
    {
        Console.Write(writeText);
        input = Console.ReadLine();
        var eror = _errorMassages.IsNullOrEmptyMassage(input);
        Console.WriteLine(eror.TextError);
        breakWhile = eror.IsSuccessfully;
    }
}

// User Methods
void PayTheDebtOff(Guid userId)
{
    ShowAllIdUserRequestLoan(userId);
    _userLoanServices.PayTheDebtOff(EnterIdRequestForTheDebtOff());
}
Guid EnterIdRequestForTheDebtOff()
{
    Console.Write("Please enter Id request: ");
    Guid IdRequest = Guid.Parse(Console.ReadLine());

    return IdRequest;
}
void ShowAllIdUserRequestLoan(Guid userId)
{
    foreach (var ii in _personsRequestsList)
    {
        if (ii.IdSender == userId && ii.StatusDuty == StatusLoan.Accepted)
        {
            Console.WriteLine(ii.Id);
            write.Status(ii.StatusDuty);
            write.WriteDutys(ii.ToString());
        }
    }
}
void StatusDuty(Guid userId)
{
    bool thePresenceOfDebt = false;

    foreach (var ii in _personsRequestsList)
    {
        if (ii.IdSender == userId)
        {
            write.Status(ii.StatusDuty);
            write.WriteDutys(ii.ToString());
            thePresenceOfDebt = true;
        }
    }
    if (!thePresenceOfDebt)
        Console.WriteLine("\nYou don't have loan");
}
void PersonalArea(int idx)
{
    if (_personsList[idx].Status == StatusUser.Accepted)
        Console.WriteLine(_personsList[idx].ToString());
    else
        throw new Exception("User is not found");
}
void MyMassages(Guid id)
{
    ShowAllIdMassagesForUser(id);
    bool presenceOfMessage = false;
    int idx = _massages.FindIndex(x => x.IdSender.Equals(id));
    foreach(var ii in _massages)
    {
        if(ii.IdRecipient == id)
        {
            Console.WriteLine(ii.ToString());
            presenceOfMessage = true;
        }
    }
    if(presenceOfMessage)
        ShowMassage();
    else
        Console.WriteLine("\nYou dot't have massages\n");
}
void ShowMassage()
{
    Guid IdMassage = Guid.Parse(Console.ReadLine());
    int idx = _massages.FindIndex(x => x.Id.Equals(IdMassage));
    Console.WriteLine($"\n{_massages[idx].ToString()}\n{_massages[idx].Text}");
}
void ShowAllIdMassagesForUser(Guid id)
{
    int idx = _massages.FindIndex(x => x.IdSender.Equals(id));
    foreach (var ii in _massages)
    {
        if (ii.IdRecipient == id)
            Console.WriteLine(ii.Id);
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
void MyMassagesManager(Guid id)
{
    ShowAllIdMassagesForManager(id);
    bool presenceOfMessages = false;
    int idx = _massages.FindIndex(x => x.IdSender.Equals(id));
    foreach (var ii in _massages)
    {
        if (ii.Role == Roles.Manager)
        {
            Console.WriteLine(ii.ToString());
            presenceOfMessages = true;
        }
    }
    if (presenceOfMessages)
        ShowMassage();
    else
        Console.WriteLine("\nYou don't have massage\n");
}
void ShowMassageManager()
{
    Guid IdMassage = Guid.Parse(Console.ReadLine());
    int idx = _massages.FindIndex(x => x.Id.Equals(IdMassage));
    Console.WriteLine($"\n{_massages[idx].ToString()}\n{_massages[idx].Text}");
}
void ShowAllIdMassagesForManager(Guid id)
{
    int idx = _massages.FindIndex(x => x.IdSender.Equals(id));
    foreach (var ii in _massages)
    {
        if (ii.Role == Roles.Manager)
            Console.WriteLine(ii.Id);
    }
}
void StatisticLoan()
{
    Console.WriteLine("Accepted Request: " + _allStatistic.countAcceptedRequestLoan);
    Console.WriteLine("Refuse Request: " + _allStatistic.countRefuseRequestLoan);
}
void ShowUsersLoan()
{
    foreach (var ii in _personsRequestsList)
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
        write.Choice(ref Id, ref Choice);
        int idx = _personsList.FindIndex(x => x.Id.Equals(Id));

        _moderatorServices.ChoiceRequest(idx, Choice);
        if (Choice == "Refuse")
        {
            _allStatistic.countRefuseRequestRegistration++;
            string textСauseRefusalOfRegistration = string.Empty;
            _personsList[idx].CauseRefuseRegistration = СauseRefusalOfRegistration(textСauseRefusalOfRegistration);
        }
        else if (Choice == "Accepted")
            _allStatistic.countAcceptedRequestRegistration++;
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
void MyMassagesAdmin(Guid id)
{
    ShowAllIdMassagesForAdmin(id);
    bool b = false;
    Console.WriteLine(_massages[0].Role);
    foreach (var ii in _massages)
    {
        if (ii.Role == Roles.Admin)
        {
            Console.WriteLine(ii.ToString());
            b = true;
        }
    }
    if (b)
        ShowMassage();
    else
        Console.WriteLine("\nYou don't have massage\n");
}
void ShowMassageAdmin()
{
    Guid IdMassage = Guid.Parse(Console.ReadLine());
    int idx = _massages.FindIndex(x => x.Id.Equals(IdMassage));
    Console.WriteLine($"\n{_massages[idx].ToString()}\n{_massages[idx].Text}");
}
void ShowAllIdMassagesForAdmin(Guid id)
{
    int idx = _massages.FindIndex(x => x.IdSender.Equals(id));
    foreach (var ii in _massages)
    {
        if (ii.Role == Roles.Admin)
            Console.WriteLine(ii.Id);
    }
}
Guid EnterId()
{
    Console.Write("Please enter id person: ");
    Guid id = Guid.Empty;
    id = Guid.Parse(Console.ReadLine());
    return id;
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
    foreach (var ii in _personsRequestsList)
    {
        if (ii.IdSender == id)
            Console.WriteLine($"Name: {ii.Name} - Id Transaction: {ii.Id}");
    }
}
void ShowAllUserIdLoan()
{
    bool presence = false;
    for (int i = 0; i < _personsRequestsList.Count; i++)
    {
        if (_personsRequestsList[i].StatusDuty == StatusLoan.Pending)
        {
            Console.WriteLine($"Name: {_personsRequestsList[i].Name} - Id: {_personsRequestsList[i].Id}");
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
    _createAdmin.RegistrationPerson(dto);
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
    whileMethodNullOrEmpty("First name: ", ref helpStr);
    inputUserDtoFirstName.FirstName = helpStr;

}
void LastName(ref InputUserDto inputUserDtoLastName)
{
    whileMethodNullOrEmpty("Last name: ", ref helpStr);
    inputUserDtoLastName.LastName = helpStr;
}
void Patronymic(ref InputUserDto inputUserDtoPatronymic)
{
    whileMethodNullOrEmpty("Patronymic: ", ref helpStr);
    inputUserDtoPatronymic.Patronymic = helpStr;
}
void DateOfBirth(ref InputUserDto inputUserDtoDateOfBirth)
{
    whileMethodNullOrEmpty("Date of birth: ", ref helpStr);
    inputUserDtoDateOfBirth.DateOfBirth = DateTime.Parse(helpStr);
}
void Login(ref InputUserDto inputUserDtoLogin)
{
    whileMethodNullOrEmpty("Login: ", ref helpStr);
    inputUserDtoLogin.Login = helpStr;
}
void Password(ref InputUserDto inputUserDtoPassword)
{
    whileMethodNullOrEmpty("Password: ", ref helpStr);
    inputUserDtoPassword.Password = helpStr;
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
    foreach (var ii in _personsRequestsList)
    {
        if(ii.StatusDuty == StatusLoan.Pending)
            Console.WriteLine($"Name: {ii.Name} - Id Transaction: {ii.Id}");
    }
}