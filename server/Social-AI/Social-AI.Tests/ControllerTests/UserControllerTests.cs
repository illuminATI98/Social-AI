using FakeItEasy;
using FluentAssertions;
using Social_AI.Services;

namespace Social_AI.Tests.ControllerTests;

public class UserControllerTests
{
    private readonly IUserService _userService;

    public UserControllerTests()
    {
        _userService = A.Fake<IUserService>();
    }
}