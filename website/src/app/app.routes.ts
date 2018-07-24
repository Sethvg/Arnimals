import {LoginComponent} from "./login/login.component";
import {Routes} from "@angular/router";
import {DashboardComponent} from "./dashboard/dashboard.component";
import {AllComponent} from "./dashboard/all/all.component";
import {AnimalComponent} from "./dashboard/animal/animal.component";
import {RewardsComponent} from "./rewards/rewards.component";
import {OrganizationComponent} from "./organization/organization.component";
import {SettingsComponent} from "./settings/settings.component";
import {DonationsComponent} from "./donations/donations.component";
import {AddComponent} from "./dashboard/animal/add/add.component";
import {TestComponent} from "./test/test.component";

export const appRoutes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'dashboard/:id',
    component: DashboardComponent,
    children: [
      {
        path: 'animals',
        component: AllComponent,
        children: [
          {
            path: 'add',
            component: AddComponent
          },
          {
            path: ':name',
            component: AnimalComponent
          }
        ]
      },
      {
        path: 'settings',
        component: SettingsComponent,

      }, {
        path: 'donations',
        component: DonationsComponent

      },
      {
        path: 'rewards',
        component: RewardsComponent,

      },

      {
        path: 'organization',
        component: OrganizationComponent,
      },
      {
        path: 'test',
        component: TestComponent,
      },

      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'animals'
      }
    ]
  },
  {
    path: '**',
    redirectTo: '/login'
  },
];
