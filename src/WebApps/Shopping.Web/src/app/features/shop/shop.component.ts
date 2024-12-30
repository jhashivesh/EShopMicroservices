import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { HttpClient } from '@angular/common/http';
import { Product } from '../../shared/models/product';
import { ProductItemComponent } from './product-item/product-item.component';

@Component({
  selector: 'app-shop',
  imports: [ProductItemComponent],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);

  private http = inject(HttpClient);
  products: Product[] = [];
  ///catalog-service/products?pageNumber=1&pageSize=2
  ngOnInit(): void {
    this.shopService.getProducts().subscribe({
      next: (response) => {
        ({ products: this.products } = response);
      },
      error: (err) => console.error('Error: ', err),
    });
  }
}
