import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { loadEmployees, deleteEmployee } from '../../state/employee.actions';
import { selectAllEmployees, selectEmployeeLoading } from '../../state/employee.selectors';
import { Router } from '@angular/router';

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.scss']
})
export class EmployeesListComponent implements OnInit {
  employees$ = this.store.select(selectAllEmployees);
  loading$ = this.store.select(selectEmployeeLoading);

  constructor(private store: Store, private router: Router) {}

  ngOnInit(): void {
    this.store.dispatch(loadEmployees());
  }

  onAdd() {
    this.router.navigate(['/employees/new']);
  }

  onEdit(id: string) {
    this.router.navigate([`/employees/${id}/edit`]);
  }

  onDelete(name: string, id: string) {
    if (confirm(`Are you sure you want to delete the employee ${name}?`)) {
      this.store.dispatch(deleteEmployee({ id }));
    }
  }
}
