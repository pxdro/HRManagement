import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { provideMockStore } from '@ngrx/store/testing';

import { DepartmentFormComponent } from './department-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('DepartmentFormComponent', () => {
  let component: DepartmentFormComponent;
  let fixture: ComponentFixture<DepartmentFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DepartmentFormComponent],
      imports: [ReactiveFormsModule, RouterTestingModule, SharedModule, BrowserAnimationsModule, NoopAnimationsModule],
      providers: [
        provideMockStore({
          initialState: {},
        }),
        {
          provide: ActivatedRoute,
          useValue: {
            paramMap: of({
              get: (_key: string) => null,
            }),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(DepartmentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
