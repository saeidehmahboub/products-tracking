import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Product } from '../../../models/product.model';

@Component({
  selector: 'app-table',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
})
export class TableComponent {
  @Input() data: any[] = [];
  @Input() headers: string[] = [];
  @Input() keys: string[] = [];
  @Input() action?: { label: string; link: string };
}
