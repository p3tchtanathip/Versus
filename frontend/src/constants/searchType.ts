export const SportsSearchType = {
    All: "All",
    Player: "Player",
    Team: "Team",
} as const;

export type SportsSearchType =
    (typeof SportsSearchType)[keyof typeof SportsSearchType];