import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as EmployeeActions from './employee.actions';
import { catchError, map, mergeMap, of } from 'rxjs';
import { EmployeeService } from 'src/app/core/services/employee.service';

@Injectable()
export class EmployeeEffects {
  constructor(
    private actions$: Actions,
    private employeeService: EmployeeService
  ) {}

  // Load all
  loadEmployees$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.loadEmployees),
      mergeMap(() =>
        this.employeeService.getEmployees().pipe(
          map(response =>
            response.errorMessage
              ? EmployeeActions.loadEmployeesFailure({ error: response.errorMessage })
              : EmployeeActions.loadEmployeesSuccess({ employees: response.data })
          ),
          catchError(() =>
            of(EmployeeActions.loadEmployeesFailure({ error: 'Server error' }))
          )
        )
      )
    )
  );

  // Load one
  loadEmployee$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.loadEmployee),
      mergeMap(({ id }) =>
        this.employeeService.getEmployee(id).pipe(
          map(response =>
            response.errorMessage
              ? EmployeeActions.loadEmployeeFailure({ error: response.errorMessage })
              : EmployeeActions.loadEmployeeSuccess({ employee: response.data })
          ),
          catchError(() =>
            of(EmployeeActions.loadEmployeeFailure({ error: 'Server error' }))
          )
        )
      )
    )
  );
  
  // Create
  addEmployee$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.addEmployee),
      mergeMap(({ employee }) =>
        this.employeeService.addEmployee(employee).pipe(
          map((response) =>
            response.errorMessage
              ? EmployeeActions.addEmployeeFailure({ error: response.errorMessage })
              : EmployeeActions.addEmployeeSuccess({ employee: response.data })
          ),
          catchError((error) =>
            of(EmployeeActions.addEmployeeFailure({ error: error.message || 'Server error' }))
          )
        )
      )
    )
  );

  addEmployeeSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.addEmployeeSuccess),
      map(() => EmployeeActions.loadEmployees())
    )
  );

  addEmployeeFailure$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.addEmployeeFailure),
      map(() => EmployeeActions.loadEmployees())
    )
  );

  // Update
  updateEmployee$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.updateEmployee),
      mergeMap(({ employee }) =>
        this.employeeService.updateEmployee(employee).pipe(
          map((response) =>
            response.errorMessage
              ? EmployeeActions.updateEmployeeFailure({ error: response.errorMessage })
              : EmployeeActions.updateEmployeeSuccess({ employee: response.data })
          ),
          catchError((error) =>
            of(EmployeeActions.updateEmployeeFailure({ error: error.message || 'Server error' }))
          )
        )
      )
    )
  );

  updateEmployeeSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.updateEmployeeSuccess),
      map(() => EmployeeActions.loadEmployees())
    )
  );

  updateEmployeeFailure$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.updateEmployeeFailure),
      map(() => EmployeeActions.loadEmployees())
    )
  );

  // Delete
  deleteEmployee$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.deleteEmployee),
      mergeMap(({ id }) =>
        this.employeeService.deleteEmployee(id).pipe(
          map(response =>
            response.errorMessage
              ? EmployeeActions.deleteEmployeeFailure({ error: response.errorMessage })
              : EmployeeActions.deleteEmployeeSuccess({ id })
          ),
          catchError(() =>
            of(EmployeeActions.deleteEmployeeFailure({ error: 'Server error' }))
          )
        )
      )
    )
  );

  deleteEmployeeSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.deleteEmployeeSuccess),
      map(() => EmployeeActions.loadEmployees())
    )
  );

  deleteEmployeeFailure$ = createEffect(() =>
    this.actions$.pipe(
      ofType(EmployeeActions.deleteEmployeeFailure),
      map(() => EmployeeActions.loadEmployees())
    )
  );
}
