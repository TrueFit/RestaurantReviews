using ErrorOr;
using MediatR;
using TrueFoodReviews.Application.Common.Interfaces.Persistence;
using TrueFoodReviews.Application.Users.Common;

namespace TrueFoodReviews.Application.Users.Queries.List;

public class ListMutedUsersQueryHandler : 
    IRequestHandler<ListMutedUsersQuery, ErrorOr<ListUsersResult>>
{
    private readonly IUserRepository _userRepository;

    public ListMutedUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<ListUsersResult>> Handle(ListMutedUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _userRepository.GetMutedUsers();
        return new ListUsersResult(users);
    }
}