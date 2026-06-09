export const TierListCategory = {
    Movie: 'Movie',
    Series: 'Series',
    Music: 'Music',
    Sports: 'Sports',
} as const;

export type TierListCategory = (typeof TierListCategory)[keyof typeof TierListCategory];