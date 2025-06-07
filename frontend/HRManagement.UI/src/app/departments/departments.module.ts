import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DepartmentsRoutingModule } from './departments-routing.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { departmentReducer } from './state/department.reducer';
import { DepartmentEffects } from './state/department.effects';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  imports: [
    CommonModule,
    DepartmentsRoutingModule,
    StoreModule.forFeature('departments', departmentReducer),
    EffectsModule.forFeature([DepartmentEffects]),
    ReactiveFormsModule 
  ],
  declarations: []
})
export class DepartmentsModule { }
