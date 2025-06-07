import { createReducer, on } from '@ngrx/store';
import * as EmployeeActions from './employee.actions';
import { Employee } from '../../shared/models/apiModels.model';

export interface EmployeeState {
  employees: Employee[];
  employee?: Employee | null;
  return: boolean;
  loading: boolean;
  error: string | null;
}

export const initialState: EmployeeState = {
  employees: [],
  employee: null,
  return: false,
  loading: false,
  error: null
};

export const employeeReducer = createReducer(
  initialState,

  // Load All
  on(EmployeeActions.loadEmployees, state => ({ ...state, loading: true, error: null })),
  on(EmployeeActions.loadEmployeesSuccess, (state, { employees }) => ({
    ...state,
    loading: false,
    employees,
    error: null
  })),
  on(EmployeeActions.loadEmployeesFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Load One
  on(EmployeeActions.loadEmployee, state => ({ ...state, loading: true, error: null })),
  on(EmployeeActions.loadEmployeeSuccess, (state, { employee }) => ({
    ...state,
    loading: false,
    employee,
    error: null
  })),
  on(EmployeeActions.loadEmployeeFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Create
  on(EmployeeActions.addEmployee, state => ({ ...state, loading: true, error: null })),
  on(EmployeeActions.addEmployeeSuccess, (state, { employee }) => ({
    ...state,
    loading: false,
    employees: [...state.employees, employee],
    error: null,
    return: true
  })),
  on(EmployeeActions.addEmployeeFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Update
  on(EmployeeActions.updateEmployee, state => ({ ...state, loading: true, error: null })),
  on(EmployeeActions.updateEmployeeSuccess, (state, { employee }) => ({
    ...state,
    loading: false,
    employees: state.employees.map(e => e.id === employee.id ? employee : e),
    error: null,
    return: true
  })),
  on(EmployeeActions.updateEmployeeFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Delete
  on(EmployeeActions.deleteEmployee, state => ({ ...state, loading: true, error: null })),
  on(EmployeeActions.deleteEmployeeSuccess, (state, { id }) => ({
    ...state,
    loading: false,
    employees: state.employees.filter(e => e.id !== id),
    error: null,
    return: true
  })),
  on(EmployeeActions.deleteEmployeeFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);
