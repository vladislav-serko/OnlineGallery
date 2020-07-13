export interface ImageInterface {
    id: string;
    userId: string;
    url: string;
    urlToFull : string;
    description?: string;
    shortDescription?:string;
    published?: string;
    likeCount: number;
    isLiked: boolean;
}
