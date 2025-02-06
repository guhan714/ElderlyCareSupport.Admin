using Bogus;
using ElderlyCareSupport.Admin.Application.IService;
using ElderlyCareSupport.Admin.Domain.Models;
using Xunit;

namespace ElderlyCareSuppport.Admin.Testing.UnitTests;

[CollectionDefinition("IUserService userService")]
public class MockUserList
{
    private readonly IUserService _userService;
    private List<User>? _users;

    public MockUserList(IUserService userService)
    {
        _userService = userService;
    }

    private List<User> GetUsersFromBogus()
    {
        var faker = new Faker<User>()
            .RuleFor(a => a.FirstName, f => f.Name.FirstName())
            .RuleFor(a => a.LastName, f => f.Name.LastName())
            .RuleFor(a => a.Email, f => f.Internet.Email())
            .RuleFor(a => a.Password, f => f.Internet.Password())
            .RuleFor(a => a.Address, f => f.Address.StreetAddress())
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.Country, f => f.Address.Country())
            .RuleFor(a => a.Region, f => f.Name.JobArea())
            .RuleFor(a => a.Gender, f => f.PickRandom("Male", "Female"))
            .RuleFor(a => a.PhoneNumber, f => long.Parse(f.Phone.PhoneNumber().Replace("-", "").Replace(" ", "")))
            .RuleFor(a => a.PostalCode, f => long.Parse(f.Address.ZipCode()));
        
        _users = faker.Generate(34);
        return _users;
    }
    
    [Fact]
    public async Task GetUserList_ShouldReturnListOfUsers_WhenUserListIsNotNull()
    {
        var uses = await _userService.GetAllUsersAsync(null);
        Assert.NotNull(uses);
        Assert.Equal(GetUsersFromBogus(), uses.Items);
    }
}