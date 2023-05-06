import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { JwtModule, JwtHelperService } from "@auth0/angular-jwt";

import { AuthenticationGuard } from './guards/auth.guard';
// Services
import { ProductService } from './services/product.service';

import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { AuthService } from './services/auth.service';
import { SubscriptionService } from './services/subscription.service';
import { SubscriptionsComponent } from './user/subscriptions/subscriptions.component';


// PrimeNG

import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { AccordionModule } from 'primeng/accordion';     //accordion and accordion tab
import { DataViewModule } from 'primeng/dataview';
import { TagModule } from 'primeng/tag';
import { TableModule } from 'primeng/table';
import { ChartModule } from 'primeng/chart';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';
import { FormsModule } from '@angular/forms';
import { config } from 'rxjs';
import { InputTextModule } from 'primeng/inputtext';

import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    SubscriptionsComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AccordionModule,
    DataViewModule,
    TagModule,
    TableModule,
    ChartModule,
    DialogModule,
    ButtonModule,
    ToolbarModule,
    InputTextModule,
    CommonModule,
    RouterModule.forRoot([
      { path: '', component: AppComponent, pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
/*      { path: 'register', component: RegisterComponent },*/
      { path: 'subscriptions', component: SubscriptionsComponent, canActivate: [AuthenticationGuard] },
    ])
  ],
  providers: [
    ProductService,
    AuthService,
    SubscriptionService,
    {
      provide: "BASE_API_URL",
      useValue: "https://localhost:62136",
      
    },
    JwtHelperService,
    AuthenticationGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
