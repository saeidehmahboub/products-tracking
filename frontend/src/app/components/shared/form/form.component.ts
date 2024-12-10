import { CommonModule, NgFor } from '@angular/common';
import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [NgFor, ReactiveFormsModule, CommonModule],
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss'],
})
export class FormComponent implements OnInit, OnChanges {
  @Input() fields: {
    name: string;
    type: string;
    label: string;
    placeholder?: string;
    options?: any[];
    validators?: any[];
  }[] = [];
  @Input() buttonLabel: string = 'Submit';
  @Input() initialValues: any = {};
  @Output() formSubmit = new EventEmitter<any>();

  formGroup!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.createForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialValues'] && this.formGroup) {
      this.formGroup.patchValue(this.initialValues);
    }
  }

  createForm(): void {
    const controls: any = {};
    this.fields.forEach((field) => {
      controls[field.name] = [
        this.initialValues[field.name] || null,
        field.validators || [],
      ];
    });
    this.formGroup = this.fb.group(controls);
  }

  onSubmit(): void {
    if (this.formGroup.valid) {
      this.formSubmit.emit(this.formGroup.value);
    }
  }
}
