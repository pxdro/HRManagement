import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectAllDepartments, selectDepartmentLoading } from '../../state/department.selectors';
import { deleteDepartment, loadDepartments } from '../../state/department.actions';

@Component({
  selector: 'app-departments-list',
  templateUrl: './departments-list.component.html',
  styleUrls: ['./departments-list.component.scss']
})
export class DepartmentsListComponent implements OnInit {
  departments$ = this.store.select(selectAllDepartments);
  loading$ = this.store.select(selectDepartmentLoading);

  constructor(private store: Store, private router: Router) {}
  
  ngOnInit(): void {
    this.store.dispatch(loadDepartments());
  }

  onAdd() {
    this.router.navigate(['/departments/new']);
  }

  onEdit(id: string) {
    this.router.navigate([`/departments/${id}/edit`]);
  }

  onDelete(name: string, id: string) {
    if (confirm(`Are you sure you want to delete the department ${name}?`)) {
      this.store.dispatch(deleteDepartment({ id }));
    }
  }
}
