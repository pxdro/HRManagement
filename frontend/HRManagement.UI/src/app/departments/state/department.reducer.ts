import { createReducer, on } from '@ngrx/store';
import * as DepartmentActions from './department.actions';
import { Department } from 'src/app/shared/models/apiModels.model';

export interface DepartmentState {
  departments: Department[];
  department?: Department | null;
  return: boolean;
  loading: boolean;
  error: string | null;
}

export const initialState: DepartmentState = {
  departments: [],
  department: null,
  return: false,
  loading: false,
  error: null
};

export const departmentReducer = createReducer(
  initialState,

  // Load All
  on(DepartmentActions.loadDepartments, state => ({ ...state, loading: true, error: null })),
  on(DepartmentActions.loadDepartmentsSuccess, (state, { departments }) => ({
    ...state,
    loading: false,
    departments,
    error: null
  })),
  on(DepartmentActions.loadDepartmentsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Load One
  on(DepartmentActions.loadDepartment, state => ({ ...state, loading: true, error: null })),
  on(DepartmentActions.loadDepartmentSuccess, (state, { department }) => ({
    ...state,
    loading: false,
    department,
    error: null
  })),
  on(DepartmentActions.loadDepartmentFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Create
  on(DepartmentActions.addDepartment, state => ({ ...state, loading: true, error: null })),
  on(DepartmentActions.addDepartmentSuccess, (state, { department }) => ({
    ...state,
    loading: false,
    departments: [...state.departments, department],
    error: null,
    return: true
  })),
  on(DepartmentActions.addDepartmentFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Update
  on(DepartmentActions.updateDepartment, state => ({ ...state, loading: true, error: null })),
  on(DepartmentActions.updateDepartmentSuccess, (state, { department }) => ({
    ...state,
    loading: false,
    departments: state.departments.map(e => e.id === department.id ? department : e),
    error: null,
    return: true
  })),
  on(DepartmentActions.updateDepartmentFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Delete
  on(DepartmentActions.deleteDepartment, state => ({ ...state, loading: true, error: null })),
  on(DepartmentActions.deleteDepartmentSuccess, (state, { id }) => ({
    ...state,
    loading: false,
    departments: state.departments.filter(e => e.id !== id),
    error: null,
    return: true
  })),
  on(DepartmentActions.deleteDepartmentFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);
