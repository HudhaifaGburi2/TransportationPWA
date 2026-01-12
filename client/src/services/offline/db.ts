import Dexie, { type Table } from 'dexie'

export interface OfflineDistrict {
    id?: number
    districtId: string
    districtNameAr: string
    districtNameEn?: string
    isActive: boolean
    synced: boolean
    lastModified: Date
}

export interface OfflineLocation {
    id?: number
    locationId: string
    locationCode: string
    locationName: string
    locationType?: string
    isActive: boolean
    synced: boolean
    lastModified: Date
}

export interface OfflineBus {
    id?: number
    busId: string
    busNumber: string
    periodId: number
    driverName?: string
    capacity: number
    isActive: boolean
    synced: boolean
    lastModified: Date
}

export interface SyncQueueItem {
    id?: number
    action: 'create' | 'update' | 'delete'
    entity: string
    entityId: string
    data: any
    createdAt: Date
    retries: number
}

export class TumsDatabase extends Dexie {
    districts!: Table<OfflineDistrict>
    locations!: Table<OfflineLocation>
    buses!: Table<OfflineBus>
    syncQueue!: Table<SyncQueueItem>

    constructor() {
        super('TumsOfflineDB')
        this.version(1).stores({
            districts: '++id, districtId, districtNameAr, isActive, synced',
            locations: '++id, locationId, locationCode, isActive, synced',
            buses: '++id, busId, busNumber, periodId, isActive, synced',
            syncQueue: '++id, action, entity, entityId, createdAt'
        })
    }
}

export const db = new TumsDatabase()

export async function addToSyncQueue(
    action: 'create' | 'update' | 'delete',
    entity: string,
    entityId: string,
    data: any
): Promise<void> {
    await db.syncQueue.add({
        action,
        entity,
        entityId,
        data,
        createdAt: new Date(),
        retries: 0
    })
}

export async function getPendingSyncItems(): Promise<SyncQueueItem[]> {
    return db.syncQueue.toArray()
}

export async function removeSyncItem(id: number): Promise<void> {
    await db.syncQueue.delete(id)
}

export async function clearAllOfflineData(): Promise<void> {
    await db.districts.clear()
    await db.locations.clear()
    await db.buses.clear()
    await db.syncQueue.clear()
}
