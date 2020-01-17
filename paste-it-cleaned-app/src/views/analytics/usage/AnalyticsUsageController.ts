import moment from 'moment'

import backend from '../../../config/backend.json'
import { BaseController } from '../../BaseController'

import { HitDailyEntity } from '../../../entities/api'
import AnalyticsUsage from './AnalyticsUsage'

export class AnalyticsUsageController extends BaseController {
    private component?: AnalyticsUsage = undefined

    constructor(component: AnalyticsUsage) {
        super()
        this.component = component
    }

    initialize = () => {
        this.initializeHitsDaily(
            moment()
                .startOf('month')
                .toDate(),
            moment()
                .endOf('month')
                .toDate()
        )
    }

    initializeHitsDaily = (startDate: Date, endDate: Date) => {
        this.component!.setState({ hitsDailyLoading: true })
        this.getHitsDailyBackend(
            startDate,
            endDate,
            (json: HitDailyEntity[]) => {
                this.component!.setState({ hitsDaily: json })
                this.createOfficeUsage()
                this.createWebsiteUsage()
                this.createLocalUsage()
                this.component!.setState({ hitsDailyLoading: false, isLoaded: true })
            },
            (e: any) => {
                this.component!.setState({ hitsDailyLoading: false, hitsDailyError: true, isLoaded: true })
            }
        )
    }

    getDaysList = () => {
        return new Array(moment(this.component!.state.startDate).daysInMonth()).fill(null).map((x, i) =>
            moment()
                .startOf('month')
                .add(i, 'days')
                .toDate()
        )
    }

    createOfficeUsage = () => {
        const officeUsage: any[] = []
        const state = this.component!.state

        this.getDaysList().forEach((e, i) => {
            const hitDaily = state.hitsDaily.find(hd => moment(hd.date).format('YYYY-MM-DD') == moment(e).format('YYYY-MM-DD'))
            officeUsage.push({
                name: (i + 1).toString(),
                Word: hitDaily ? hitDaily.countWord : 0,
                Excel: hitDaily ? hitDaily.countExcel : 0,
                PowerPoint: hitDaily ? hitDaily.countPowerPoint : 0,
            })
        })

        this.component!.setState({ officeUsage })
    }

    createWebsiteUsage = () => {
        const websiteUsage: any[] = []
        const state = this.component!.state

        this.getDaysList().forEach((e, i) => {
            const hitDaily = state.hitsDaily.find(hd => moment(hd.date).format('YYYY-MM-DD') == moment(e).format('YYYY-MM-DD'))
            websiteUsage.push({
                name: (i + 1).toString(),
                Web: hitDaily ? hitDaily.countWeb : 0,
            })
        })

        this.component!.setState({ websiteUsage })
    }

    createLocalUsage = () => {
        const localUsage: any[] = []
        const state = this.component!.state

        this.getDaysList().forEach((e, i) => {
            const hitDaily = state.hitsDaily.find(hd => moment(hd.date).format('YYYY-MM-DD') == moment(e).format('YYYY-MM-DD'))
            localUsage.push({
                name: (i + 1).toString(),
                Text: hitDaily ? hitDaily.countText : 0,
                Image: hitDaily ? hitDaily.countImage : 0,
            })
        })

        this.component!.setState({ localUsage })
    }

    previous = () => {
        const newStartDate = moment(this.component!.state.startDate)
            .subtract(1, 'month')
            .toDate()
        const newEndDate = moment(newStartDate)
            .endOf('month')
            .toDate()

        this.component!.setState({ startDate: newStartDate, endDate: newEndDate })
        this.initializeHitsDaily(newStartDate, newEndDate)
    }

    next = () => {
        const newStartDate = moment(this.component!.state.startDate)
            .add(1, 'month')
            .toDate()
        const newEndDate = moment(newStartDate)
            .endOf('month')
            .toDate()

        this.component!.setState({ startDate: newStartDate, endDate: newEndDate })
        this.initializeHitsDaily(newStartDate, newEndDate)
    }

    private getHitsDailyBackend = (startDate: Date, endDate: Date, success?: (json: any) => void, error?: (e: any) => void) => {
        this.callBackend(this.endpoint(`/hits/daily?startDate=${moment(startDate).format('YYYY-MM-DD')}&endDate=${moment(endDate).format('YYYY-MM-DD')}`), 'GET', success, error)
    }

    private endpoint(path: string) {
        return this.culturedEndpoint(backend.endpoints.analytics + path)
    }
}
