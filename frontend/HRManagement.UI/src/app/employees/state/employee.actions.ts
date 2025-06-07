import { createAction, props } from '@ngrx/store';
import { Employee } from '../../shared/models/apiModels.model';

// Load all
export const loadEmployees = createAction('[Employee] Load Employees');
export const loadEmployeesSuccess = createAction('[Employee] Load Employees Success', props<{ employees: Employee[] }>());
export const loadEmployeesFailure = createAction('[Employee] Load Employees Failure', props<{ error: string }>());

// Load one
export const loadEmployee = createAction('[Employee] Load Employee', props<{ id: string }>());
export const loadEmployeeSuccess = createAction('[Employee] Load Employee Success', props<{ employee: Employee }>());
export const loadEmployeeFailure = createAction('[Employee] Load Employee Failure', props<{ error: string }>());

// Create
export const addEmployee = createAction('[Employee] Add Employee', props<{ employee: Employee }>());
export const addEmployeeSuccess = createAction('[Employee] Add Employee Success', props<{ employee: Employee }>());
export const addEmployeeFailure = createAction('[Employee] Add Employee Failure', props<{ error: string }>());

// Update
export const updateEmployee = createAction('[Employee] Update Employee', props<{ employee: Employee }>());
export const updateEmployeeSuccess = createAction('[Employee] Update Employee Success', props<{ employee: Employee }>());
export const updateEmployeeFailure = createAction('[Employee] Update Employee Failure', props<{ error: string }>());

// Delete
export const deleteEmployee = createAction('[Employee] Delete Employee', props<{ id: string }>());
export const deleteEmployeeSuccess = createAction('[Employees] Delete Employee Success',props<{ id: string }>());
export const deleteEmployeeFailure = createAction('[Employee] Delete Employee Failure', props<{ error: string }>());
