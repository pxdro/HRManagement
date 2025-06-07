import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DepartmentsRoutingModule } from './departments-routing.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { departmentReducer } from './state/department.reducer';
import { DepartmentEffects } from './state/department.effects';
import { ReactiveFormsModule } from '@angular/forms';
import { DepartmentsListComponent } from './components/departments-list/departments-list.component';
import { DepartmentFormComponent } from './components/department-form/department-form.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  imports: [
    CommonModule,
    DepartmentsRoutingModule,
    StoreModule.forFeature('departments', departmentReducer),
    EffectsModule.forFeature([DepartmentEffects]),
    ReactiveFormsModule,
    SharedModule
  ],
  declarations: [
    DepartmentsListComponent,
    DepartmentFormComponent
  ]
})
export class DepartmentsModule { }
