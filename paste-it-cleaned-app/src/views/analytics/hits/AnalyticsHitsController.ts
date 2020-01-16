import moment from 'moment'

import backend from '../../../config/backend.json'
import { BaseController } from '../../BaseController'

import { createData } from '../../../helpers/AnalyticsHelper'
import { Hit, ActionResult } from '../../../entities/api'
import AnalyticsHits from './AnalyticsHits'

export class AnalyticsHitsController extends BaseController {
    private component?: AnalyticsHits = undefined

    constructor(component: AnalyticsHits) {
        super()
        this.component = component
    }

    initialize = () => {
        this.initializeHits(1, 200)
    }

    initializeHits = (page: number, pageSize: number) => {
        this.component!.setState({ hitsLoading: true })
        this.getHits(
            page,
            pageSize,
            (json: ActionResult) => {
                let hits: any[] = []
                json.results.forEach((hit: Hit) => {
                    hits.push(createData(moment(hit.date!).toDate(), hit.ip!, hit.type!, hit.userAgent!, hit.price!))
                })
                this.component!.setState({ hits, hitsLoading: false })
            },
            (e: any) => {
                this.component!.setState({ hitsLoading: false, hitsError: true })
            }
        )
    }

    getHits = (page: number, pageSize: number, success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint(`/hits?page=${page}&pageSize=${pageSize}`), 'GET', success, error)
    }

    endpoint(path: string) {
        return this.culturedEndpoint(backend.analytics.endpoint + path)
    }
}
