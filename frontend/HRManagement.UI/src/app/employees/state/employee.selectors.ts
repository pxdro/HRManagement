import { createFeatureSelector, createSelector } from '@ngrx/store';
import { EmployeeState } from './employee.reducer';

// Employees full state
export const selectEmployeeState = createFeatureSelector<EmployeeState>('employees');

// All employees list
export const selectAllEmployees = createSelector(
  selectEmployeeState,
  (state) => state.employees
);

// Loading
export const selectEmployeeLoading = createSelector(
  selectEmployeeState,
  (state) => state.loading
);

// Error
export const selectEmployeeError = createSelector(
  selectEmployeeState,
  (state) => state.error
);

// Select employee by Id
export const selectEmployeeById = (id: string) =>
  createSelector(
    selectAllEmployees,
    (employees) => employees.find(emp => emp.id === id)
  )