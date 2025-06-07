import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { take } from 'rxjs';
import { DepartmentFormControls } from 'src/app/shared/models/formsControl.model';
import { selectDepartmentById } from '../../state/department.selectors';
import { addDepartment, updateDepartment } from '../../state/department.actions';

@Component({
  selector: 'app-department-form',
  templateUrl: './department-form.component.html',
  styleUrls: ['./department-form.component.scss']
})
export class DepartmentFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  private departmentId?: string;
  
  constructor(
    private fb: FormBuilder,
    private store: Store,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  get f() {
    return this.form.controls as unknown as DepartmentFormControls;
  }

  ngOnInit(): void {
    this.initForm();
    
    this.route.paramMap.pipe(take(1)).subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.departmentId = id;

        this.store.select(selectDepartmentById(id)).pipe(take(1)).subscribe(department => {
          if (department) {
            this.form.patchValue({
              name: department.name,
              description: department.description,
            });
          } else {
            this.router.navigate(['/departments']);
          }
        });
      }
    });
  }

  private initForm() {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(150)]],
      description: ['', Validators.maxLength(250)],
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value;

    if (this.isEditMode && this.departmentId) {
      this.store.dispatch(updateDepartment({
        department: {
          id: this.departmentId,
          ...formValue,
        }
      }));
    } else {
      this.store.dispatch(addDepartment({ department: formValue }));
    }

    this.router.navigate(['/departments']);
  }

  onCancel() {
    this.router.navigate(['/departments']);
  }

}
