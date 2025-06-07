import { createFeatureSelector, createSelector } from '@ngrx/store';
import { DepartmentState } from './department.reducer';

// Departments full state
export const selectDepartmentState = createFeatureSelector<DepartmentState>('departments');

// All departments list
export const selectAllDepartments = createSelector(
  selectDepartmentState,
  (state) => state.departments
);

// Loading
export const selectDepartmentLoading = createSelector(
  selectDepartmentState,
  (state) => state.loading
);

// Error
export const selectDepartmentError = createSelector(
  selectDepartmentState,
  (state) => state.error
);

// Select department by Id
export const selectDepartmentById = (id: string) =>
  createSelector(
    selectAllDepartments,
    (departments) => departments.find(emp => emp.id === id)
  )