import { createAction, props } from '@ngrx/store';
import { Department } from 'src/app/shared/models/apiModels.model';

// Load all
export const loadDepartments = createAction('[Department] Load Departments');
export const loadDepartmentsSuccess = createAction('[Department] Load Departments Success', props<{ departments: Department[] }>());
export const loadDepartmentsFailure = createAction('[Department] Load Departments Failure', props<{ error: string }>());

// Load one
export const loadDepartment = createAction('[Department] Load Department', props<{ id: string }>());
export const loadDepartmentSuccess = createAction('[Department] Load Department Success', props<{ department: Department }>());
export const loadDepartmentFailure = createAction('[Department] Load Department Failure', props<{ error: string }>());

// Create
export const addDepartment = createAction('[Department] Add Department', props<{ department: Department }>());
export const addDepartmentSuccess = createAction('[Department] Add Department Success', props<{ department: Department }>());
export const addDepartmentFailure = createAction('[Department] Add Department Failure', props<{ error: string }>());

// Update
export const updateDepartment = createAction('[Department] Update Department', props<{ department: Department }>());
export const updateDepartmentSuccess = createAction('[Department] Update Department Success', props<{ department: Department }>());
export const updateDepartmentFailure = createAction('[Department] Update Department Failure', props<{ error: string }>());

// Delete
export const deleteDepartment = createAction('[Department] Delete Department', props<{ id: string }>());
export const deleteDepartmentSuccess = createAction('[Departments] Delete Department Success',props<{ id: string }>());
export const deleteDepartmentFailure = createAction('[Department] Delete Department Failure', props<{ error: string }>());
