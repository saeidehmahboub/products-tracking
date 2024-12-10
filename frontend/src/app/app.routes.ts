import { Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { FormComponent } from './components/shared/form/form.component';
import { EventComponent } from './components/event/event.component';
import { ReportComponent } from './components/report/report.component';

export const routes: Routes = [
  { path: 'products', component: ProductListComponent },
  { path: 'events', component: EventComponent },
  { path: 'dashboard', component: ReportComponent },
  { path: 'edit/:id', component: FormComponent },
  { path: '**', redirectTo: '/products' },
];
