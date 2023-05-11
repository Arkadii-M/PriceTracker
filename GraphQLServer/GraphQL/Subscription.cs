using DTO.GraphQL;
using GraphQLServer.Services;

namespace GraphQLServer.GraphQL
{
    public class GraphQlSubscription
    {
        [Subscribe]
        [Topic(nameof(Mutation.AddHistory))]
        public HistoryQLPayload OnHistoryAdded([EventMessage] long historyId, [Service]IHistoryService historyService) => historyService.GetHistoryById(historyId);
    }
}
