import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map, Observable, publish } from 'rxjs';
import { NewSubscription, Subscription } from '../dto/subscription.dto';
import { Apollo, gql } from 'apollo-angular';

@Injectable()
export class SubscriptionService {

  constructor(private apollo: Apollo) {
  }

  //private formHeader(): HttpHeaders {
  //  return new HttpHeaders({
  //    'Content-Type': 'application/json',
  //    'Authorization': 'Bearer ' + (localStorage.getItem('token') ?? '')
  //  });
  //}

  getUserSubscriptions(): Observable<Subscription[]> {
    return this.apollo.query({
      query: gql`{
           userSubscriptions{
              subscriptionId
              userId
              productId
              checkMinutes
            }
        }`
    }).pipe(map((data) => { return (data.data as any).userSubscriptions as Subscription[] }));
  }

  addSubscription(link: string, checkTime: number) {
    return this.apollo.mutate({
      mutation: gql`
        mutation AddNewSub($link:String!,$checkPeriod: Int!)
        {
          addSubscriptionByLink(sub: {link: $link,checkMinutes: $checkPeriod}) {
            subscriptionId
            userId
            productId
            checkMinutes

          }
        }`,
      variables:
      {
        link: link,
        checkPeriod: checkTime,
      }

    }).subscribe(res => console.log(res), err => console.error(err));
  }
}
