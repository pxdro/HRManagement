import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EmployeesListComponent } from './employees-list.component';
import { provideMockStore } from '@ngrx/store/testing';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { selectAllEmployees, selectEmployeeLoading } from '../../state/employee.selectors';
import { SharedModule } from 'src/app/shared/shared.module';

describe('EmployeesListComponent', () => {
  let component: EmployeesListComponent;
  let fixture: ComponentFixture<EmployeesListComponent>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [EmployeesListComponent],
      providers: [
        provideMockStore({
          selectors: [
            { selector: selectAllEmployees, value: [] },
            { selector: selectEmployeeLoading, value: false },
          ],
        }),
        { provide: Router, useValue: mockRouter }
      ],
      imports: [SharedModule]
    }).compileComponents();

    fixture = TestBed.createComponent(EmployeesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
