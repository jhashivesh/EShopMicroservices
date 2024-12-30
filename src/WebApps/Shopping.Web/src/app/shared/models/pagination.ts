export type Pagination<T> = {
  pageIndex: number;
  pageSize: number;
  count: number;
  products: T[];
};
