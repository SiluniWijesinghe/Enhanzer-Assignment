import { Component, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';
import { PurchaseBillService } from '../../core/services/purchase-bill.service';
import { PurchaseBillLine, UserLocation } from '../../core/models/models';

@Component({
  selector: 'app-purchase-bill',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './purchase-bill.component.html',
  styleUrl: './purchase-bill.component.css'
})
export class PurchaseBillComponent implements OnInit {
  items = signal<string[]>([]);
  locations = signal<UserLocation[]>([]);
  lines = signal<PurchaseBillLine[]>([]);
  isSubmitting = signal(false);
  submitMessage = signal<string | null>(null);

  form;

  // Live preview of Total Cost / Total Selling as the user types,
  // using the exact same formula the backend re-applies on submit:
  //   Total Cost    = (Standard Cost x Qty) - Discount%
  //   Total Selling = Standard Price x Qty
  totalItems = computed(() => this.lines().length);
  totalQuantity = computed(() => this.lines().reduce((sum, l) => sum + l.qty, 0));
  totalCostSum = computed(() => this.lines().reduce((sum, l) => sum + l.totalCost, 0));
  totalSellingSum = computed(() => this.lines().reduce((sum, l) => sum + l.totalSelling, 0));

  liveTotalCost = 0;
  liveTotalSelling = 0;

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly purchaseBillService: PurchaseBillService,
    private readonly router: Router
  ) {
    this.form = this.fb.group({
      item: ['', Validators.required],
      batch: ['', Validators.required],
      standardCost: [0, [Validators.required, Validators.min(0)]],
      standardPrice: [0, [Validators.required, Validators.min(0)]],
      qty: [1, [Validators.required, Validators.min(1)]],
      freeQty: [0, [Validators.required, Validators.min(0)]],
      discount: [0, [Validators.required, Validators.min(0), Validators.max(100)]]
    });
  }

  ngOnInit(): void {

    forkJoin({
      items: this.purchaseBillService.getItems(),
      locations: this.purchaseBillService.getLocations()
    }).subscribe(({ items, locations }) => {
      this.items.set(items);
      this.locations.set(locations.length ? locations : this.authService.getCachedLocations());
    });

    this.form.valueChanges.subscribe(() => this.recalculateLivePreview());
  }

  private recalculateLivePreview(): void {
    const v = this.form.getRawValue();
    const cost = v.standardCost ?? 0;
    const price = v.standardPrice ?? 0;
    const qty = v.qty ?? 0;
    const discount = v.discount ?? 0;

    const gross = cost * qty;
    this.liveTotalCost = Math.round((gross - gross * (discount / 100)) * 100) / 100;
    this.liveTotalSelling = Math.round(price * qty * 100) / 100;
  }

  addLine(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.getRawValue();
    this.recalculateLivePreview();

    const line: PurchaseBillLine = {
      item: v.item!,
      batch: v.batch!,
      standardCost: v.standardCost!,
      standardPrice: v.standardPrice!,
      qty: v.qty!,
      freeQty: v.freeQty!,
      discount: v.discount!,
      totalCost: this.liveTotalCost,
      totalSelling: this.liveTotalSelling
    };

    this.lines.update((current) => [...current, line]);

    this.form.reset({
      item: '',
      batch: '',
      standardCost: 0,
      standardPrice: 0,
      qty: 1,
      freeQty: 0,
      discount: 0
    });
  }

  removeLine(index: number): void {
    this.lines.update((current) => current.filter((_, i) => i !== index));
  }

  submitBill(): void {
    if (!this.lines().length) return;

    this.isSubmitting.set(true);
    this.submitMessage.set(null);

    this.purchaseBillService.submit(this.lines()).subscribe({
      next: (response) => {
        this.isSubmitting.set(false);
        this.submitMessage.set(`Purchase bill #${response.purchaseBillId} saved successfully.`);
        this.lines.set([]);
      },
      error: () => {
        this.isSubmitting.set(false);
        this.submitMessage.set('Something went wrong while saving. Please try again.');
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
