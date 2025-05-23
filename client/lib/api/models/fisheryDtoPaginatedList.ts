/**
 * Generated by orval v7.9.0 🍺
 * Do not edit manually.
 * Fishio API
 * OpenAPI spec version: v1
 */
import type { FisheryDto } from './fisheryDto';

export interface FisheryDtoPaginatedList {
  /** @nullable */
  items?: FisheryDto[] | null;
  pageNumber?: number;
  readonly totalPages?: number;
  readonly totalCount?: number;
  readonly hasPreviousPage?: boolean;
  readonly hasNextPage?: boolean;
}
