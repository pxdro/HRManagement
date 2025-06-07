import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as DepartmentActions from './department.actions';
import { catchError, map, mergeMap, of } from 'rxjs';
import { DepartmentService } from 'src/app/core/services/department.service';

@Injectable()
export class DepartmentEffects {
  constructor(
    private actions$: Actions,
    private departmentService: DepartmentService
  ) {}

  // Load all
  loadDepartments$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.loadDepartments),
      mergeMap(() =>
        this.departmentService.getDepartments().pipe(
          map(response =>
            response.errorMessage
              ? DepartmentActions.loadDepartmentsFailure({ error: response.errorMessage })
              : DepartmentActions.loadDepartmentsSuccess({ departments: response.data })
          ),
          catchError(() =>
            of(DepartmentActions.loadDepartmentsFailure({ error: 'Server error' }))
          )
        )
      )
    )
  );

  // Load one
  loadDepartment$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.loadDepartment),
      mergeMap(({ id }) =>
        this.departmentService.getDepartment(id).pipe(
          map(response =>
            response.errorMessage
              ? DepartmentActions.loadDepartmentFailure({ error: response.errorMessage })
              : DepartmentActions.loadDepartmentSuccess({ department: response.data })
          ),
          catchError(() =>
            of(DepartmentActions.loadDepartmentFailure({ error: 'Server error' }))
          )
        )
      )
    )
  );
  
  // Create
  addDepartment$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.addDepartment),
      mergeMap(({ department }) =>
        this.departmentService.addDepartment(department).pipe(
          map((response) =>
            response.errorMessage
              ? DepartmentActions.addDepartmentFailure({ error: response.errorMessage })
              : DepartmentActions.addDepartmentSuccess({ department: response.data })
          ),
          catchError((error) =>
            of(DepartmentActions.addDepartmentFailure({ error: error.message || 'Server error' }))
          )
        )
      )
    )
  );
  
  addDepartmentSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.addDepartmentSuccess),
      map(() => DepartmentActions.loadDepartments())
    )
  );

  addDepartmentFailure$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.addDepartmentFailure),
      map(() => DepartmentActions.loadDepartments())
    )
  );

  // Update
  updateDepartment$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.updateDepartment),
      mergeMap(({ department }) =>
        this.departmentService.updateDepartment(department).pipe(
          map((response) =>
            response.errorMessage
              ? DepartmentActions.updateDepartmentFailure({ error: response.errorMessage })
              : DepartmentActions.updateDepartmentSuccess({ department: response.data })
          ),
          catchError((error) =>
            of(DepartmentActions.updateDepartmentFailure({ error: error.message || 'Server error' }))
          )
        )
      )
    )
  );
  
  updateDepartmentSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.updateDepartmentSuccess),
      map(() => DepartmentActions.loadDepartments())
    )
  );

  updateDepartmentFailure$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.updateDepartmentFailure),
      map(() => DepartmentActions.loadDepartments())
    )
  );

  // Delete
  deleteDepartment$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.deleteDepartment),
      mergeMap(({ id }) =>
        this.departmentService.deleteDepartment(id).pipe(
          map(response =>
            response.errorMessage
              ? DepartmentActions.deleteDepartmentFailure({ error: response.errorMessage })
              : DepartmentActions.deleteDepartmentSuccess({ id })
          ),
          catchError(() =>
            of(DepartmentActions.deleteDepartmentFailure({ error: 'Server error' }))
          )
        )
      )
    )
  );
  
  deleteDepartmentSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.deleteDepartmentSuccess),
      map(() => DepartmentActions.loadDepartments())
    )
  );
  
  deleteDepartmentFailure$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DepartmentActions.deleteDepartmentFailure),
      map(() => DepartmentActions.loadDepartments())
    )
  );
}
