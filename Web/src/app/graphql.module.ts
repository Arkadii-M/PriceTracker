import {NgModule} from '@angular/core';
import {ApolloModule, APOLLO_OPTIONS} from 'apollo-angular';
import {ApolloClientOptions, ApolloLink, InMemoryCache, split} from '@apollo/client/core';
import { HttpLink } from 'apollo-angular/http';
import { setContext } from '@apollo/client/link/context';
import { WebSocketLink } from '@apollo/client/link/ws';
import { getMainDefinition } from '@apollo/client/utilities';

const uri = 'http://localhost:52176/graphql'; // <-- add the URL of the GraphQL server here
const ws_uri = 'ws://localhost:52176/graphql';
export function createApollo(httpLink: HttpLink): ApolloClientOptions<any> {
  // Create a WebSocket link:
  const ws = new WebSocketLink({
    uri: ws_uri,
    options: {
      reconnect: true,
    },
  });

  const basic = setContext((operation, context) => ({
    headers: {
      'Content-Type': 'application/json',
    },
  }));

  const auth = setContext((operation, context) => {
    const token = localStorage.getItem('token');

    if (token === null) {
      return {};
    } else {
      return {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      };
    }
  });

  // using the ability to split links, you can send data to each link
  // depending on what kind of operation is being sent
  const link = split(
    // split based on operation type
    ({ query }) => {
      let definition = getMainDefinition(query);
      return definition.kind === 'OperationDefinition' && definition.operation === 'subscription';
    },
    ws,
    ApolloLink.from([basic, auth, httpLink.create({ uri:uri })]),
  );
  return {
    link: link,
    cache: new InMemoryCache(),
  };
}
//export function createApollo(httpLink: HttpLink): ApolloClientOptions<any> {

//  const http = httpLink.create({
//    uri: uri,
//  });

//  // Create a WebSocket link:
//  const ws = new WebSocketLink({
//    uri: ws_uri,
//    options: {
//      reconnect: true,
//    },
//  });

//  const basic = setContext((operation, context) => ({
//    headers: {
//      'Content-Type': 'application/json',
//    },
//  }));
//  const auth = setContext((operation, context) => {
//    const token = localStorage.getItem('token');

//    if (token === null) {
//      return {};
//    } else {
//      return {
//        headers: {
//          Authorization: `Bearer ${token}`,
//        },
//      };
//    }
//  });

//  // using the ability to split links, you can send data to each link
//  // depending on what kind of operation is being sent
//  const split_link = split(
//    // split based on operation type
//    ({ query }) => {
//      let definition = getMainDefinition(query);
//      return definition.kind === 'OperationDefinition' && definition.operation === 'subscription';
//    },
//    ws,
//    http,
//  );

//  const link = ApolloLink.from([basic, auth, split_link, httpLink.create({ uri })]);
//  return {
//    link: link,
//    cache: new InMemoryCache(),
//  };
//}

@NgModule({
  exports: [ApolloModule],
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: createApollo,
      deps: [HttpLink],
    },
  ],
})
export class GraphQLModule {}
