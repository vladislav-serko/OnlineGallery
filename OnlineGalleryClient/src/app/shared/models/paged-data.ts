export interface PagedData<T>{
    data : T[],
    pageCount : number;
    totalItems : number;
    itemCount: number;
    currentPage: number;
}
