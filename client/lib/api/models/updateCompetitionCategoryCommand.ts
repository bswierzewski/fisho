/**
 * Generated by orval v7.9.0 🍺
 * Do not edit manually.
 * Fishio API
 * OpenAPI spec version: v1
 */

export interface UpdateCompetitionCategoryCommand {
  competitionId?: number;
  competitionCategoryId?: number;
  /** @nullable */
  customNameOverride?: string | null;
  /** @nullable */
  customDescriptionOverride?: string | null;
  sortOrder?: number;
  isPrimaryScoring?: boolean;
  maxWinnersToDisplay?: number;
  isEnabled?: boolean;
  /** @nullable */
  fishSpeciesId?: number | null;
}
