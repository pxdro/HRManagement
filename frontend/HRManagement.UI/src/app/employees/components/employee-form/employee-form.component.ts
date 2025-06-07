import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Department } from '../../../shared/models/apiModels.model';
import { take } from 'rxjs/operators';
import { selectAllDepartments } from 'src/app/departments/state/department.selectors';
import { loadDepartments } from 'src/app/departments/state/department.actions';
import { selectEmployeeById } from '../../state/employee.selectors';
import { addEmployee, updateEmployee } from '../../state/employee.actions';
import { EmployeeFormControls } from 'src/app/shared/models/formsControl.model';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss']
})
export class EmployeeFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  departments$: Observable<Department[]>;
  private employeeId?: string;

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.departments$ = this.store.select(selectAllDepartments);
  }

  get f() {
    return this.form.controls as unknown as EmployeeFormControls;
  }

  ngOnInit(): void {
    this.initForm();
    
    this.store.dispatch(loadDepartments());
    
    this.route.paramMap.pipe(take(1)).subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.employeeId = id;

        this.store.select(selectEmployeeById(id)).pipe(take(1)).subscribe(employee => {
          if (employee) {
            this.form.patchValue({
              name: employee.name,
              email: employee.email,
              password: employee.password,
              position: employee.position,
              hireDate: employee.hireDate ? new Date(employee.hireDate).toISOString().substring(0, 10) : '',
              isAdmin: employee.isAdmin,
              departmentId: employee.departmentId,
            });
          } else {
            this.router.navigate(['/employees']);
          }
        });
      }
    });
  }

  private initForm() {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(150)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      position: ['', [Validators.required, Validators.maxLength(150)]],
      hireDate: ['', Validators.required],
      isAdmin: [false],
      departmentId: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value;

    if (this.isEditMode && this.employeeId) {
      this.store.dispatch(updateEmployee({
        employee: {
          id: this.employeeId,
          ...formValue,
        }
      }));
    } else {
      this.store.dispatch(addEmployee({ employee: formValue }));
    }

    this.router.navigate(['/employees']);
  }

  onCancel() {
    this.router.navigate(['/employees']);
  }
}
